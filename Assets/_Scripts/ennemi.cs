using System.Collections;
using System.Collections.Generic;
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
    public float distance;
    public float vitesseDeplacement;
    [Header(         "Raycast")]
    public float distanceRaycast;
    public float distanceRaycastCote;
    public float raycastDecalement;
    public bool debugRaycast;

    //variable privée
    private InfoEvent _evennement = new InfoEvent();
    private GameObject _renderer;
    Statistiques statsRef;
    private Vector3 _deplacement;


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
        distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));

        if (distance < 0.6) {
            if ((Teddy.transform.position.x - gameObject.transform.position.x) > 0) {
                _deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, 0, 0);
            }
            else {
                _deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, 0, 0);
            }
        }
        else if (distance > 1.1 && pointsVie == pointsVieMax && !estUnique) {
            Mort(false);
        }


        /************************************************Gestion de la détection avec le sol************************************************/
        RaycastHit raycast_0;
        RaycastHit raycast_1;
        RaycastHit raycast_2;

        //détection du raycast_0 raycast
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.back), out raycast_0, distanceRaycast)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.back) * raycast_0.distance, Color.yellow);
        }
        //détection du raycast_1 raycast
        else if (Physics.Raycast(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back), out raycast_1, distanceRaycastCote)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * raycast_1.distance, Color.green);
        }
        //détection du raycast_2 raycast
        else if (Physics.Raycast(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back), out raycast_2, distanceRaycastCote)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * raycast_2.distance, Color.blue);
        }
        //si aucun raycast ne touche de sols
        else {
            _deplacement.y = -2;
            if (debugRaycast) {
                Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * distanceRaycastCote, Color.red);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.back) * distanceRaycast, Color.red);
                Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * distanceRaycastCote, Color.red);
            };
        }



        gameObject.GetComponent<Rigidbody>().velocity = _deplacement;
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
    void Mort(bool recompenseTrigger = true) {
        if (recompenseTrigger == true) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, _evennement);
        }
        if (DoitRecompenser(chanceLoot) && recompenseTrigger == true) {
            var recompenseTemp = Instantiate(recompense);
            recompenseTemp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageYRecompense, gameObject.transform.position.z);
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
