using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SystemeEventsLib;

public class joueur : MonoBehaviour {

    public int pointDeVie;
    public int pointDeVieMax;
    public float forceSaut = 0.005f;
    public float multiplicateurGravite;
    public float multiplicateurSautMin;
    public float vitesseDeplacement;
    public float distanceRaycast;
    public float distanceRaycastCote;
    public float raycastDecalement;
    public AudioClip SonSaut;
    public Transform armeRef;
    public ArmeTemplate armeActuelle;
    public List<string> inventaireObjet = new List<string>();
    public List<int> inventaireObjetQte = new List<int>();
    public List<string> inventaireArme = new List<string>();
    public List<ArmeTemplate> inventaireArmeTemplates = new List<ArmeTemplate>();

    private Rigidbody _RB;
    private Animator _animator;
    private AudioSource _AS;
    private bool _estTouchable = true;
    private bool _estEnLair = false;
    private bool _utiliseTrampoline = false;
    private GameObject _teddyRenderer;

    InfoEvent infoEvent = new InfoEvent();

    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;
    public bool debugRaycast;

    // Use this for initialization
    void Start() {
        _RB = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _AS = Camera.main.gameObject.GetComponent<AudioSource>();
        armeRef = gameObject.transform.GetChild(3);
        pointDeVie = pointDeVieMax;
        _teddyRenderer = gameObject.transform.GetChild(2).gameObject;
        ChangementArme();
        infoEvent.HPMax = pointDeVieMax;
    }

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

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down), out raycast_0, distanceRaycast)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast_0.distance, Color.yellow);
            _estEnLair = false;
            _animator.SetBool("saut", false);
            _animator.SetBool("chute", false);
            if (raycast_0.collider.gameObject.name == "trampoline") {
                _RB.velocity = Vector3.zero;
                _RB.AddForce(1, 4, 0, ForceMode.Impulse);
                _utiliseTrampoline = true;
            }
            else {
                _utiliseTrampoline = false;
            }  
        }
        else if (Physics.Raycast(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down), out raycast_1, distanceRaycastCote)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * raycast_1.distance, Color.green);
            _estEnLair = false;
            _animator.SetBool("chute", false);
            if (raycast_1.collider.gameObject.name == "trampoline") {
                _RB.velocity = Vector3.zero;
                _RB.AddForce(1, 4, 0, ForceMode.Impulse);
                _utiliseTrampoline = true;
            }
            else {
                _utiliseTrampoline = false;
            }
        }
        else if (Physics.Raycast(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down), out raycast_2, distanceRaycastCote)) {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * raycast_2.distance, Color.blue);
            _estEnLair = false;
            _animator.SetBool("chute", false);
            if (raycast_2.collider.gameObject.name == "trampoline") {
                _RB.velocity = Vector3.zero;
                _RB.AddForce(1, 4, 0, ForceMode.Impulse);
                _utiliseTrampoline = true;
            }
            else {
                _utiliseTrampoline = false;
            }
        }
        else {
            if (debugRaycast) {
                Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * distanceRaycastCote, Color.red);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * distanceRaycast, Color.red);
                Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.down) * distanceRaycastCote, Color.red);
            };
            _animator.SetBool("chute", true);
            _estEnLair = true;
        }

        /**************************************************combat*******************************************************************************/
        if (Input.GetButton("Gauche") || Input.GetButton("Droite")) {
            armeRef.gameObject.SetActive(true);
        }

        /**********************************************************débug************************************************************************/
        if (debugVelocite) debug(debugX, debugY, debugZ);

        /**********************************************************déplacements*****************************************************************/
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
        if (collision.gameObject.tag == "recompense") {
            if (collision.gameObject.name != "poulet") {
                int resultat = EstDansLinventaire(collision.gameObject.name);
                if (resultat != -1) {
                    inventaireObjetQte[resultat]++;
                }
                else {
                    inventaireObjet.Insert(inventaireObjet.Count, collision.name);
                    inventaireObjetQte.Insert(inventaireObjetQte.Count, 1);
                }
            }
            else {
                pointDeVie += 2;
                if (pointDeVie > pointDeVieMax) {
                    pointDeVie = pointDeVieMax;
                }
                infoEvent.HP = pointDeVie;
                SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "arme") {
            int resultat = EstDansLinventaire(collision.gameObject.name);
            if (resultat == -1) {
                int position = inventaireArme.Count;
                inventaireArme.Insert(position, collision.gameObject.name);
                
                inventaireArmeTemplates.Insert(position, Resources.Load<ArmeTemplate>("armes/"+ collision.gameObject.name));
            }
            Destroy(collision.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "ennemi" && _estTouchable == true) {
            pointDeVie -= collision.gameObject.GetComponent<ennemi>().degats;
            infoEvent.HP = pointDeVie;
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            StartCoroutine("DevienIntouchable", 2f);
            VerificationMortTeddy();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "ennemi" && _estTouchable == true) {
            pointDeVie -= collision.gameObject.GetComponent<ennemi>().degats;
            infoEvent.HP = pointDeVie;
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent);
            StartCoroutine("DevienIntouchable", 2f);
            VerificationMortTeddy();
        }
    }



    /**************************************************************Fonctions********************************************************************/
    void VerificationMortTeddy(){
        if (pointDeVie <= 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    int EstDansLinventaire(string cible) {
        var taille = inventaireObjet.Count;
        for (int i = 0; i < taille; i++) {
            if (inventaireObjet[i] == cible) {
                return i;
            }
        }
        return -1;
    }

    int EstDansLinventaireArme(string cible) {
        var taille = inventaireArme.Count;
        for (int i = 0; i < taille; i++) {
            if (inventaireObjet[i] == cible) {
                return i;
            }
        }
        return -1;
    }

    void ChangementArme() {
        armeRef.GetComponent<arme>().degats = armeActuelle.degat;
        armeRef.GetComponent<MeshFilter>().mesh = armeActuelle.objet.GetComponent<MeshFilter>().sharedMesh;
        armeRef.GetComponent<CapsuleCollider>().radius = armeActuelle.objet.GetComponent<CapsuleCollider>().radius;
        armeRef.GetComponent<CapsuleCollider>().height = armeActuelle.objet.GetComponent<CapsuleCollider>().height;
        armeRef.GetComponent<CapsuleCollider>().isTrigger = true;
        armeRef.GetComponent<CapsuleCollider>().center = armeActuelle.objet.GetComponent<CapsuleCollider>().center;
        armeRef.GetComponent<CapsuleCollider>().direction = armeActuelle.objet.GetComponent<CapsuleCollider>().direction;
        armeRef.gameObject.transform.localEulerAngles = armeActuelle.objet.transform.localEulerAngles;
        armeRef.gameObject.transform.localScale = armeActuelle.objet.transform.localScale;
        armeRef.gameObject.name = armeActuelle.nom;
    }

    public void ChangementArme(string armeTemplate) {
        int resultat = EstDansLinventaireArme(armeTemplate);
        if (resultat != -1) {
            armeActuelle = inventaireArmeTemplates[resultat];
        }
        armeRef.GetComponent<arme>().degats = armeActuelle.degat;
        armeRef.GetComponent<MeshFilter>().mesh = armeActuelle.objet.GetComponent<MeshFilter>().sharedMesh;
        armeRef.GetComponent<CapsuleCollider>().radius = armeActuelle.objet.GetComponent<CapsuleCollider>().radius;
        armeRef.GetComponent<CapsuleCollider>().height = armeActuelle.objet.GetComponent<CapsuleCollider>().height;
        armeRef.GetComponent<CapsuleCollider>().isTrigger = true;
        armeRef.GetComponent<CapsuleCollider>().center = armeActuelle.objet.GetComponent<CapsuleCollider>().center;
        armeRef.GetComponent<CapsuleCollider>().direction = armeActuelle.objet.GetComponent<CapsuleCollider>().direction;
        armeRef.gameObject.transform.localEulerAngles = armeActuelle.objet.transform.localEulerAngles;
        armeRef.gameObject.transform.localScale = armeActuelle.objet.transform.localScale;
        armeRef.gameObject.name = armeActuelle.nom;
    }

    //IENUMERATORS
    private IEnumerator DevienIntouchable(float temps) {
        _estTouchable = false;
        InvokeRepeating("IndicateurDegat", 0f, 0.15f);
        yield return new WaitForSeconds(temps);
        _estTouchable = true;
        CancelInvoke("IndicateurDegat");
        _teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
        StopCoroutine("DevienIntouchable");
    }

    private void IndicateurDegat() {
        _teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled = !_teddyRenderer.GetComponent<SkinnedMeshRenderer>().enabled;
    }

    //fonction de débug
    public void debug(bool x, bool y, bool z) {
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}
