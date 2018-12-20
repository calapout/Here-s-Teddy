using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le déplacement, la rotation et la gravite de l'auto f1
/// </summary>
/// <remarks>Auteurs: Jimmy Tremblay-Bernier et Yoann Paquette</remarks>

public class f1Deplacement : MonoBehaviour {
    [HideInInspector]
    public GameObject Teddy; //référence au gameObject de Teddy
    public float vitesseDeplacement; //stocke la vitesse de déplacement de la voiture

    [Header("Raycast")]
    public float distanceRaycast; //contient la taille du raycast principal
    public float distanceRaycastCote; //contient la taille des raycasts secondaires
    public float raycastDecalement; //contient la distancede décalement des raycasts secondaires
    public bool debugRaycast; //permet de débug ou non les raycast


    private Vector3 _deplacement; //contient les données nécessaire au déplacement
    private Ennemi _ennemi; //contient une référence au script Ennemi
    private Animator animControl; //contient une référence à l'animator
    private bool enMouvement = false; //permet de savoir si la voiture avant au non

    // assigne quelques références
    void Start () {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        animControl = GetComponent<Animator>();
    }

    // Gère le déplacement de la voiture et sa destruction si Teddy est trop loins
    void Update() {
        float distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));
        
        //si teddy est suffisamment proche il fonce vers lui
        if (distance < 0.6) {
            if ((Teddy.transform.position.x - gameObject.transform.position.x) > 0) {
                _deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, 0, 0);
            }
            else {
                _deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, 0, 0);
            }
        }
        //si teddy est trop loins il se détruit
        else if (distance > 1.1 && _ennemi.pointsVie == _ennemi.pointsVieMax && !_ennemi.estUnique) {
            _ennemi.Mort(false);
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
        //si aucun raycast ne touche de sols alors on applique une gravité
        else {
            _deplacement.y = -2;
            if (debugRaycast) {
                Debug.DrawRay(gameObject.transform.position - (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * distanceRaycastCote, Color.red);
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.back) * distanceRaycast, Color.red);
                Debug.DrawRay(gameObject.transform.position + (Vector3.right * raycastDecalement), gameObject.transform.TransformDirection(Vector3.back) * distanceRaycastCote, Color.red);
            };
        }



        gameObject.GetComponent<Rigidbody>().velocity = _deplacement;
        ControlAnimation();
    }

    /// <summary>
    /// Gère le controle de l'animation des roues et de la rotation
    /// </summary>
    void ControlAnimation() {
        animControl.SetFloat("vitesse", _deplacement.x);

        if (_deplacement.x != 0) {
            enMouvement = true;
        }

        if (animControl.GetBool("enMouvement") != enMouvement) {
            animControl.SetBool("enMouvement", enMouvement);
        }
    }
}
