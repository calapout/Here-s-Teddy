using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementAvion : MonoBehaviour {
    public GameObject Teddy;
    public float distance;
    public float vitesseDeplacement;

    public bool estEnAttaque = false;
    public GameObject arme;
    public bool tourne = false;

    private Vector3 _deplacement;
    private Ennemi _ennemi;
    private Transform _enfant;

    // Use this for initialization
    void Start() {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        _enfant = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update() {
        distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));
        if (estEnAttaque == false && (Teddy.transform.position.x > transform.position.x - 0.3 || Teddy.transform.position.x > transform.position.x + 0.3)) {
            Attaquer();
        }
        if (distance < 1.2) {
            tourne = false;
        }
        else if (distance > 1.2) {
            if (tourne == false) {
                Tourner();
            }
        }
        
        Deplacement();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "Player") {
            Tourner();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag != "Player") {
            Tourner();
        }
    }

    void Deplacement() {
        //Debug.Log(_enfant.rotation.y);
        //Debug.Log(_enfant.localRotation.y);
        if (_enfant.eulerAngles.y/90 == 1) {
            //_deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, transform.position.y, 0.795f);
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(vitesseDeplacement * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
        else {
            //_deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, transform.position.y, 0.795f);
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(-vitesseDeplacement * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
    }

    void Tourner() {

        int rotation;
        if (Teddy.transform.position.x - gameObject.transform.position.x > 0) {
            rotation = 90;
        }
        else {
            rotation = 270;
        }

        if (_enfant.eulerAngles.y != rotation) {
            tourne = true;
            _enfant.eulerAngles = new Vector3(0, rotation, 0);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void Attaquer() {
        estEnAttaque = true;
        GameObject bombe = Instantiate(arme);
        bombe.GetComponent<Bombe>().degats = _ennemi.degats;
        bombe.GetComponent<Bombe>().parent = gameObject;
        bombe.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f, gameObject.transform.position.z);
        //arme
    }
}
