using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clownDeplacement : MonoBehaviour {
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
    private Transform _enfant;
    public bool _solTouche;

    // Use this for initialization
    void Start() {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        _enfant = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update() {
        distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));

        if (distance < 0.3) {
            _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, _enfant.localEulerAngles.y + 5, _enfant.localEulerAngles.z);
            vitesseDeplacement = 7;
            Deplacement();
        }
        else if (distance < 0.7) {
            //animControl.SetBool("TeddyEstProche", false);
            if (_solTouche == true) {
                Debug.Log(_solTouche);
                gameObject.GetComponent<Rigidbody>().AddForce(0, 0.5f, 0, ForceMode.VelocityChange);
                _solTouche = false;
            }
            vitesseDeplacement = 15;
            Deplacement();
            if (Teddy.transform.position.x - transform.position.x > 0) {
                _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, 0, _enfant.localEulerAngles.z);
            }
            else {
                _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, 180, _enfant.localEulerAngles.z);
            }
        }
        else if (distance > 1.3 && _ennemi.pointsVie == _ennemi.pointsVieMax && !_ennemi.estUnique) {
            _ennemi.Mort(false);
        }

        gameObject.GetComponent<Rigidbody>().velocity = _deplacement;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.layer != 8) {
            Debug.Log("L'OSTIE DE CLOWN");
            _solTouche = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.layer != 8) {
            Debug.Log("L'OSTIE DE CLOWN");
            _solTouche = true;
        }
    }

    void Deplacement() {
        if ((Teddy.transform.position.x - gameObject.transform.position.x) > 0) {
            _deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else {
            _deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
    }
}
