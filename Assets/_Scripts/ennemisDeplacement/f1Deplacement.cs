using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class f1Deplacement : MonoBehaviour {
    public GameObject Teddy;
    public float distance;
    public float vitesseDeplacement;

    [Header("Raycast")]
    public float distanceRaycast;
    public float distanceRaycastCote;
    public float raycastDecalement;
    public bool debugRaycast;

    private Vector3 _deplacement;
    private Ennemi _ennemi;
    private Animator animControl;
    private bool enMouvement = false;

    // Use this for initialization
    void Start () {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        animControl = GetComponent<Animator>();
    }

// Update is called once per frame
void Update () {
        distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));

        if (distance < 0.6) {
            if ((Teddy.transform.position.x - gameObject.transform.position.x) > 0) {
                _deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, 0, 0);
            }
            else {
                _deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, 0, 0);
            }
        }
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
        ControlAnimation();
    }


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
