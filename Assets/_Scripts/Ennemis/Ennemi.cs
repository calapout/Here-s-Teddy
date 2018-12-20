using System.Collections;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/// <summary>
/// Classe gérant l'ennemi attacher.
/// Gestion des points de vie, des récompenses et vérifie certaines conditions
/// </summary>
public class Ennemi : MonoBehaviour {
    //variable publique
    [Header("   Condition Spéciale")]
    public bool estUnique; //condition spéciale, ne permet pas le respawn
    public bool estPoulet; //condition spéciale, Uniquement pour le BOSS

    [Header("   Gestion de la vie")]
    public int pointsVie; //pointDeVieActuel
    public int pointsVieMax; //pointDeVieMax

    [Header("   Gestion du combat")]
    public int degats; //dégat de l'ennemi
    public int experience; //expérience gagner lors du meurtre
    public GameObject spawner; //référence au spawner relié

    [Header("       Gestion des récompense")]
    public GameObject recompense; //prefab de la récompense à gagner
    public float decalageYRecompense; //décalage en y de la récompense au cas où
    public int chanceLoot; //pourcentage de chance de looter

    [Header("   Gestion des sons")]
    public AudioClip sonDegat; //son lors de dégats reçu

    [Header("   Débug")]
    public bool kill; //tue l'ennemi sans donner d'exp


    //variable privée
    private AudioSource AS; // référence à l'audioSource de la caméra
    private GameObject Teddy; //référence à Teddy
    private InfoEvent _evennement = new InfoEvent(); //instance d'infoEvent
    private GameObject _renderer; //Référence au renderer, un gameObject enfant pour le clignotement excepté pour le boss
    private Statistiques statsRef; //référence au script de stats


    // évênnement de départ
    private void Start() {
        Teddy = GameObject.Find("Teddy");
        _evennement.Experience = experience;
        _evennement.Nom = gameObject.name;
        pointsVie = pointsVieMax;
        //si on est pas le boss on fait la référence
        if (!estPoulet) {
            _renderer = transform.GetChild(0).gameObject;
        }
        //si on est unique alors pas besoin de spawner, un seul suffit :P
        if (estUnique == true) {
            spawner = null;
        }
        statsRef = Teddy.GetComponent<Statistiques>();
        AS = Camera.main.GetComponent<AudioSource>();
    }

    //Permet de tuer l'ennemi en cochant la case kill dans l'inspecteur
    private void Update() {
        if (kill == true && !estUnique && !estPoulet) {
            Mort(false);
        }
    }

    /***************************************************collision********************************************************/
    /// <summary>
    /// Lors de la colision avec une arme, un son est jouer et les dégats sont appliquers
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision) {
        //si collision avec un arme, on retire des points de vie
        if (collision.gameObject.tag == "arme") {
            AS.PlayOneShot(sonDegat, 0.3f);
            pointsVie -= Teddy.GetComponent<joueur>().armeRef.gameObject.GetComponent<arme>().degatsTotal;
            if (pointsVie <= 0) {
                Mort();
            }
            else {
                if (!estPoulet) {
                    StartCoroutine("Clignotement", 0.5f);
                }
            }
        }
    }

    /// <summary>
    /// Gère la Mort de l'Ennemi sous certaines conditions
    /// </summary>
    /// <param name="recompenseTrigger">permet de définir si on peut recevoir les loot. De base à true</param>
    public void Mort(bool recompenseTrigger = true) {

        //Envoie un event de mort d'ennemi lootable
        if (recompenseTrigger == true) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, _evennement);
        }
        //si on suffisament de chance (Random.Range) et qu'on a le droit de looter
        if (DoitRecompenser(chanceLoot) && recompenseTrigger == true) {
            var recompenseTemp = Instantiate(recompense);
            //détecte le sol sous l'ennemi lors de la mort
            RaycastHit raycast;
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out raycast, 1)) {
                if (true) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast.distance, Color.yellow);
                recompenseTemp.transform.position = new Vector3(raycast.point.x, raycast.point.y + 0.05f, raycast.point.z);
            }
            //sinon on l'instancie ou l'ennemi est mort
            else {
                recompenseTemp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageYRecompense, gameObject.transform.position.z);
            }
            recompenseTemp.name = recompense.name;
        }

        // si l'ennemi est unique, on se doit de le faire savoir par un event
        if (estUnique) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiUniqueEvent, _evennement);
        }
        // si on est le poulet, on se doit d'enclencher une mort dramatique par Animator
        else if (estPoulet) {
            gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("mort");
        }
        //sinon on dit au spawner qu'il peut relancer l'usine
        else {
            spawner.GetComponent<spawner>().estMort = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Lance un test aléatoire pour voir si on on a de la chance. Comme lors d'un lancer de d100 dans un jeu de rôle
    /// </summary>
    /// <param name="pourcentage">Pourcentage de chance de looter</param>
    /// <returns>Retourne si on peut looter(true) ou pas(false).</returns>
    bool DoitRecompenser(int pourcentage) {
        int aleatoire = Random.Range(0, 101);
        return aleatoire <= pourcentage + (statsRef.Chance.Stat - statsRef.chance);
    }

    /// <summary>
    /// inverse l'état du renderer
    /// </summary>
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
        InvokeRepeating("IndicateurDegat", 0f, 0.15f);
        yield return new WaitForSeconds(temps);
        CancelInvoke("IndicateurDegat");
        _renderer.SetActive(true);
        StopCoroutine("DevienIntouchable");
    }
}
