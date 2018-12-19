using System.Collections;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe gérant l'ennemi attacher.
 * @author Jimmy Tremblay-Bernier
 */
public class Ennemi : MonoBehaviour {
    //variable publique
    public bool estUnique;
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
    private Statistiques statsRef;


    // évênnement de départ
    private void Start() {
        Teddy = GameObject.Find("Teddy");
        _evennement.Experience = experience;
        _evennement.Nom = gameObject.name;
        pointsVie = pointsVieMax;
        _renderer = transform.GetChild(0).gameObject;
        statsRef = Teddy.GetComponent<Statistiques>();
        if (estUnique == true) {
            spawner = null;
        }
    }

    //boucle de mise à jour
    private void Update() {
        if (kill == true && !estUnique) {
            Mort(false);
        }
    }

    /***************************************************collision********************************************************/
    private void OnTriggerEnter(Collider collision) {
        //si collision avec un arme, on retire des points de vie
        if (collision.gameObject.tag == "arme") {
            pointsVie -= Teddy.GetComponent<joueur>().armeRef.gameObject.GetComponent<arme>().degatsTotal;
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
    public void Mort(bool recompenseTrigger = true) {
        if (recompenseTrigger == true) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, _evennement);
        }
        if (DoitRecompenser(chanceLoot) && recompenseTrigger == true) {
            var recompenseTemp = Instantiate(recompense);
            RaycastHit raycast;
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out raycast, 1)) {
                if (true) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast.distance, Color.yellow);
                recompenseTemp.transform.position = new Vector3(raycast.point.x, raycast.point.y + 0.05f, raycast.point.z);
            }
            else {
                recompenseTemp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageYRecompense, gameObject.transform.position.z);
            }
            recompenseTemp.name = recompense.name;
        }
        if (estUnique) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiUniqueEvent, _evennement);
        }
        else {
            spawner.GetComponent<spawner>().estMort = true;
            Destroy(gameObject);
        }
    }

    /***
     * Permet de tuer l'ennemi
     * @param int [pourcentage de chance]
     * @return bool [true si on doit donner la récompense, sinon false]
     */
    bool DoitRecompenser(int pourcentage) {
        int aleatoire = Random.Range(0, 101);
        return aleatoire <= pourcentage + (statsRef.Chance.Stat - statsRef.chance);
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
