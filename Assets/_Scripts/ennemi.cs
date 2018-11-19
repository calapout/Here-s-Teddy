using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe gérant l'ennemi attacher.
 * @author Jimmy Tremblay-Bernier
 */
public class ennemi : MonoBehaviour {
    //variable publique
    public bool kill;
    public int pointsVie;
    public int pointsVieMax;
    public int degats;
    public GameObject Teddy;
    public int experience;
    public GameObject spawner;
    public GameObject recompense;
    public float decalageYRecompense;
    public int chanceLoot;

    //variable privée
    private InfoEvent _evennement = new InfoEvent();
    private GameObject _renderer;

    //boucle de mise à jour
    private void Update() {
        if (kill == true || (Vector3.Distance(Teddy.transform.position, gameObject.transform.position) > 1.1 && pointsVie == pointsVieMax)) {
            Mort(false);
        }
    }

    // évênnement de départ
    private void Start() {
        Teddy = GameObject.Find("Teddy");
        _evennement.Experience = experience;
        pointsVie = pointsVieMax;
        _renderer = transform.GetChild(0).gameObject;
    }
    /***************************************************collision********************************************************/
    private void OnTriggerEnter(Collider collision) {
        //si collision avec un arme, on retire des points de vie
        if (collision.gameObject.tag == "arme") {
            pointsVie -= Teddy.GetComponent<joueur>().armeRef.gameObject.GetComponent<arme>().degats;
            if (pointsVie <= 0) {
                Mort();
            }
            else {
                StartCoroutine("Clignotement", 0.5f);
            }
        }
    }

    /***
     * Permet de tuer l'ennemi
     * @param bool [permet de définir si on peut recevoir les loot] de base à true
     * @return void
     */
    void Mort(bool recompenseTrigger = true) {
        if (recompenseTrigger == true) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, _evennement);
        }
        if (DoitRecompenser(chanceLoot) && recompenseTrigger == true) {
            var recompenseTemp = Instantiate(recompense);
            recompenseTemp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageYRecompense, gameObject.transform.position.z);
            recompenseTemp.name = recompense.name;
        }
        spawner.GetComponent<spawner>().estMort = true;
        Destroy(gameObject);
    }

    /***
     * Permet de tuer l'ennemi
     * @param int [pourcentage de chance]
     * @return bool [true si on doit donner la récompense, sinon false]
     */
    bool DoitRecompenser(int pourcentage) {
        int aleatoire = Random.Range(0, 101);
        return aleatoire <= pourcentage;
    }





    private void IndicateurDegat() {
        _renderer.SetActive(!_renderer.activeSelf);
    }

    //IENUMERATORS

    /***
     * Rend teddy intouchable pendant le nombre de seconde entrer en paramètre
     * @param float [nombre de seconde de l'invincibilité]
     * @return yield [WaitForSeconds]
     */
    private IEnumerator Clignotement(float temps) {
        Debug.Log("ici");
        InvokeRepeating("IndicateurDegat", 0f, 0.15f);
        yield return new WaitForSeconds(temps);
        CancelInvoke("IndicateurDegat");
        _renderer.SetActive(true);
        StopCoroutine("DevienIntouchable");
    }
}
