using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SystemeEventsLib;

/***
 * Classe controlant les mouvements du personnage, ses points de vies et son inventaire.
 * @author Jimmy Tremblay-Bernier
 * @author Yoann paquette
 * @author Catherine Beaudoin-Rheault
 * @version 2018-12-20
 */
public class joueur : MonoBehaviour {

    // variables publiques
    [Header("   Gestion de la santé")]
    public int pointDeVie; //points de vie actuel
    public int pointDeVieMax; //points de vie max
    [Header("   Gestion des déplacements")]
    public float forceSaut = 0.005f; //force de saut
    public float multiplicateurGravite; //gravite
    public float multiplicateurSautMin; //hauteur de saut minimum
    public float vitesseDeplacement; //vitesse de déplacement
    [Header("       Gestion des raycasts")]
    public float distanceRaycast; //distance du raycast principal
    public float distanceRaycastCote; //distance des raycasts secondaires
    public float raycastDecalement; //décalement des raycasts secondaires
    [Header("   Gestion des sons")]
    public AudioClip SonSaut;//sont de saut
    public AudioClip sonCollision;//son pour lorsque Teddy est touché
    public AudioClip sonArmes;//son pour l'arme
    public AudioClip sonBobine;//son pour la bobine
    public AudioClip sonTrampoline;//son pour le trampoline
    [Header("   Gestion de l'inventaire")]
    public ArmeTemplate armeActuelle; //référence
    public List<string> inventaireArme = new List<string>() {"crayon"}; //Liste de string contenant le nom des armes
    public List<ArmeTemplate> inventaireArmeTemplates = new List<ArmeTemplate>(); //Liste de ArmeTemplate contenant les scriptableObjects
    public Transform armeRef; //référence au gameObejct de l'arme

    // variables privée
    private Rigidbody _RB; //référence au rigidbody
    private Animator _animator; //référence à l'animator
    private AudioSource _AS; //référence à l'audioSource
    private GameObject _teddyRenderer; //référence au renderer de teddy
    private Rage rageRef; //référence au script de rage
    private bool _estTouchable = true; //condition pour savoir si teddy peut être toucher
    private bool _estEnLair = false; //condition pour savoir si teddy est en l'air
    private bool _utiliseTrampoline = false; //condition pour savoir si teddy utilise la trampoline

    //variable d'évennements
    private InfoEvent infoEvent = new InfoEvent();
    private InfoEvent initInfoEvent = new InfoEvent();

    //debugger
    [Header("   Débug")]
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;
    public bool debugRaycast;

    //préInitialisation ce certaines données
    void Awake()
    {
        rageRef = gameObject.GetComponent<Rage>();
        initInfoEvent.Cible = gameObject;
        initInfoEvent.HP = pointDeVie;
        initInfoEvent.HPMax = pointDeVieMax;
        initInfoEvent.Rage = rageRef.pointsDeRage;
        initInfoEvent.RageMax = rageRef.pointsDeRageMax;
        SystemeEvents.Instance.LancerEvent(NomEvent.initEvent, initInfoEvent);
    }

    // vérification de la condition de chargement de sauvegarde en local
    //assignation des références
    void Start() {
        if(PlayerPrefs.GetInt("chargerSauvegarde") == 1) {
            SystemeEvents.Instance.LancerEvent(NomEvent.chargerEvent, new InfoEvent());
        }
        _RB = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _AS = Camera.main.gameObject.GetComponent<AudioSource>();
        armeRef = gameObject.transform.GetChild(3);
        pointDeVie = pointDeVieMax;
        _teddyRenderer = gameObject.transform.GetChild(2).gameObject;
        ChangementArme();
        infoEvent.HPMax = pointDeVieMax;
    }

    // Gère le déplacement, le saut, l'orientations, la détection avec le sol, le combat, les animations et le débug
    void Update() {
        Vector3 deplacement = _RB.velocity;

        /******************************************************déplacements************************************************************/
        float deplacementHorizontale = Input.GetAxisRaw("Horizontal");
        //float deplacementVerticale = Input.GetAxisRaw("Vertical");
        deplacement.x = deplacementHorizontale * vitesseDeplacement;

        /******************************************************saut**********************************************************************/

        //détection du saut
        if (Input.GetButtonDown("Jump") && _estEnLair == false) {
            _animator.SetBool("saut", true);
            deplacement.y = forceSaut;
            _AS.PlayOneShot(SonSaut, 0.5f);
        }

        // Gestion du saut
        if (_RB.velocity.y < 0) {
            deplacement.y += Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
        }
        else if (_RB.velocity.y > 0 && !Input.GetButton("Jump")) {
            deplacement.y += Physics.gravity.y * (multiplicateurSautMin - 1) * Time.deltaTime;
        }
        /******************************************************Orientation*****************************************************************/
        if (Input.GetAxis("Horizontal") < 0) {
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0) {
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        /************************************************Gestion de la détection avec le sol************************************************/
        RaycastHit raycast_0;
        RaycastHit raycast_1;
        RaycastHit raycast_2;

        //détection du raycast_0 raycast
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down), out raycast_0, distanceRaycast)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast_0.distance, Color.yellow);
            _estEnLair = false;
            _animator.SetBool("saut", false);
            _animator.SetBool("chute", false);
        }
        //détection du raycast_1 raycast
        else if (Physics.Raycast(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down), out raycast_1, distanceRaycastCote)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * raycast_1.distance, Color.green);
            _estEnLair = false;
            _animator.SetBool("chute", false);
        }
        //détection du raycast_2 raycast
        else if (Physics.Raycast(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down), out raycast_2, distanceRaycastCote)) {
            _estEnLair = false;
            _animator.SetBool("chute", false);
            /*condition de débogage*/if (debugRaycast) Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * raycast_2.distance, Color.blue);
        }
        //si aucun raycast ne touche de sols
        else {
            _animator.SetBool("chute", true);
            _estEnLair = true;
            /*condition de débogage des raycast*/
            if (debugRaycast) {
                Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * distanceRaycastCote, Color.red);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * distanceRaycast, Color.red);
                Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * distanceRaycastCote, Color.red);
            };
        }

        /**************************************************combat*******************************************************************************/
        if (Input.GetButtonDown("Gauche")) {
            armeRef.gameObject.SetActive(true);
            _AS.PlayOneShot(sonArmes, 0.5f);
        }

        /**********************************************************débug************************************************************************/
        if (debugVelocite) debug(debugX, debugY, debugZ);

        /************************************************animation déplacements*****************************************************************/
        if (deplacement.x != 0) {
            _animator.SetBool("deplacement", true);
        }
        else {
            _animator.SetBool("deplacement", false);
        }

        if (_utiliseTrampoline == false) {
            _RB.velocity = deplacement;
        }
    }


    /**************************************************Détection des collisions*****************************************************************/

    private void OnTriggerEnter(Collider collision) {
        //si l'objet est une récompense
        if (collision.gameObject.tag == "recompense") {
            //et que c'est la bobine alors on monte les points de vie
            if (collision.gameObject.name == "bobine") {
                pointDeVie += 2;
                if (pointDeVie > pointDeVieMax) {
                    pointDeVie = pointDeVieMax;
                }
                infoEvent.HP = pointDeVie;
                SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
                _AS.PlayOneShot(sonBobine, 1f);
            }
            Destroy(collision.gameObject);
        }
        //sinon si l'objet est une arme on l'ajoute à l'inventaire des armes si elle n'est pas présente
        else if (collision.gameObject.tag == "armeLoot") {
            int resultat = EstDansLinventaireArme(collision.gameObject.name);
            if (resultat == -1) {
                int position = inventaireArme.Count;
                inventaireArme.Insert(position, collision.gameObject.name);

                inventaireArmeTemplates.Insert(position, Resources.Load<ArmeTemplate>("Armes/" + collision.gameObject.name));
                ChangementArme(collision.gameObject.name);
            }
            Destroy(collision.gameObject);
        }
        //détecte la trampoline
        else if (collision.gameObject.name == "trampoline") {
            _RB.velocity = Vector3.zero;
            _RB.AddForce(1, 4.2f, 0, ForceMode.Impulse);
            _utiliseTrampoline = true;
            _AS.PlayOneShot(sonTrampoline, 0.5f);

        }
        //détecte la sortie de la trempoline
        else if (collision.gameObject.name == "sortieTrampoline") {
            _utiliseTrampoline = false;
        }

    }

    /// <summary>
    /// Si il y a collision avec un ennemi ou une bombe, alors on applique les dégats
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        //si il y a collision avec un ennemi, alors on va rendre teddy invincible pendant 2 secondes et le faire clignoter tout en lui enlever des points de vies et le son de collision va jouer
        if ((collision.gameObject.tag == "ennemi") && _estTouchable == true) {
            pointDeVie -= collision.gameObject.GetComponent<Ennemi>().degats;
            infoEvent.HP = pointDeVie;
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            UpdateRage();
            StartCoroutine("DevienIntouchable", 2f);
            VerificationMortTeddy();
            _AS.PlayOneShot(sonCollision, 0.2f);
        }
        else if (collision.gameObject.tag == "bombe") {
            pointDeVie -= collision.gameObject.GetComponent<Bombe>().degats;
            infoEvent.HP = pointDeVie;
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            UpdateRage();
            StartCoroutine("DevienIntouchable", 2f);
            VerificationMortTeddy();
            _AS.PlayOneShot(sonCollision, 0.2f);
        }
    }

    /// <summary>
    /// si teddy reste coller, alors on applique encore plus de dégats
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "ennemi" && _estTouchable == true) {
            pointDeVie -= collision.gameObject.GetComponent<Ennemi>().degats;
            infoEvent.HP = pointDeVie;
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            UpdateRage();
            StartCoroutine("DevienIntouchable", 2f);
            VerificationMortTeddy();
            _AS.PlayOneShot(sonCollision, 0.2f);
        }
    }



    /**************************************************************Fonctions********************************************************************/
    
    void UpdateRage()
    {
        rageRef.pointsDeRage = rageRef.GainRage(Rage.TypeGain.Degats);
        rageRef.RageEventSetup();
    }
        
    /***
     * verifie si teddy est négatif en points de vies
     * @param void
     * @return void
     */
    void VerificationMortTeddy() {
        if (pointDeVie <= 0) {
            SceneManager.LoadScene("Fin");
        }
    }

    /***
     * verifie si l'arme est dans l'inventaire
     * @param string [le nom de l'arme à chercher dans le tableau arme]
     * @return [la position ou -1 pour false]
     */
    int EstDansLinventaireArme(string cible) {
        var taille = inventaireArme.Count;
        for (int i = 0; i < taille; i++) {
            if (inventaireArme[i] == cible) {
                return i;
            }
        }
        return -1;
    }

    /***
     * Gère le changement d'arme. Change divers paramètres lier à celui-ci.
     * @param void
     * @return void
     */
    void ChangementArme() {
        armeRef.GetComponent<arme>().degats = armeActuelle.degat;
        armeRef.GetComponent<MeshFilter>().mesh = armeActuelle.objet.GetComponent<MeshFilter>().sharedMesh;
        armeRef.GetComponent<MeshRenderer>().sharedMaterial = armeActuelle.objet.GetComponent<MeshRenderer>().sharedMaterial;
        armeRef.GetComponent<CapsuleCollider>().radius = armeActuelle.objet.GetComponent<CapsuleCollider>().radius;
        armeRef.GetComponent<CapsuleCollider>().height = armeActuelle.objet.GetComponent<CapsuleCollider>().height;
        armeRef.GetComponent<CapsuleCollider>().isTrigger = true;
        armeRef.GetComponent<CapsuleCollider>().center = armeActuelle.objet.GetComponent<CapsuleCollider>().center;
        armeRef.GetComponent<CapsuleCollider>().direction = armeActuelle.objet.GetComponent<CapsuleCollider>().direction;
        armeRef.gameObject.transform.localEulerAngles = armeActuelle.objet.transform.localEulerAngles;
        armeRef.gameObject.transform.localScale = armeActuelle.objet.transform.localScale;
        armeRef.gameObject.name = armeActuelle.nom;
        armeRef.gameObject.tag = "arme";
    }

    /***
     * Fonction de surcharge de ChangementArme(); Elle recherche aussi l'arme dans l'inventaire;
     * @param string [nom de l'arme à recherher et à utiliser]
     * @return void
     */
    public void ChangementArme(string armeTemplate) {
        int resultat = EstDansLinventaireArme(armeTemplate);
        if (resultat != -1) {
            armeActuelle = inventaireArmeTemplates[resultat];
        }
        armeRef.GetComponent<arme>().degats = armeActuelle.degat;
        armeRef.GetComponent<MeshFilter>().mesh = armeActuelle.objet.GetComponent<MeshFilter>().sharedMesh;
        armeRef.GetComponent<MeshRenderer>().sharedMaterial = armeActuelle.objet.GetComponent<MeshRenderer>().sharedMaterial;
        armeRef.GetComponent<CapsuleCollider>().radius = armeActuelle.objet.GetComponent<CapsuleCollider>().radius;
        armeRef.GetComponent<CapsuleCollider>().height = armeActuelle.objet.GetComponent<CapsuleCollider>().height;
        armeRef.GetComponent<CapsuleCollider>().isTrigger = true;
        armeRef.GetComponent<CapsuleCollider>().center = armeActuelle.objet.GetComponent<CapsuleCollider>().center;
        armeRef.GetComponent<CapsuleCollider>().direction = armeActuelle.objet.GetComponent<CapsuleCollider>().direction;
        armeRef.gameObject.transform.localEulerAngles = armeActuelle.objet.transform.localEulerAngles;
        armeRef.gameObject.transform.localScale = armeActuelle.objet.transform.localScale;
        armeRef.gameObject.name = armeActuelle.nom;
        armeRef.gameObject.tag = "arme";
    }

    /***
     * Fait clignoter teddy
     * @param void
     * @return void
     */
    private void IndicateurDegat() {
        _teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled = !_teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled;
    }

    //IENUMERATORS

    /***
     * Rend teddy intouchable pendant le nombre de seconde entrer en paramètre
     * @param float [nombre de seconde de l'invincibilité]
     * @return yield [WaitForSeconds]
     */
    private IEnumerator DevienIntouchable(float temps) {
        _estTouchable = false;
        InvokeRepeating("IndicateurDegat", 0f, 0.15f);
        yield return new WaitForSeconds(temps);
        _estTouchable = true;
        CancelInvoke("IndicateurDegat");
        _teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
        StopCoroutine("DevienIntouchable");
    }



    //fonction de débug

    /***
     * gère le débug de la vélocité selon 3 booléen
     * @param bool [si on débug la vélocité en x], bool [si on débug la vélocité en y], bool [si on débug la vélocité en z]
     * @return yield [WaitForSeconds]
     */
    public void debug(bool x, bool y, bool z) {
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}
