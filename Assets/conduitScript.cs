using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conduitScript : MonoBehaviour {
    public bool estDetruit = false;
    private Transform[] _enfant = new Transform[7];

    private void Start() {
        for (int i = 0; i < 7; i++) {
            _enfant[i] = gameObject.transform.GetChild(i);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "arme" && estDetruit == false) {
            GererDetruireGrille(true);
        }
    }

    public void GererDetruireGrille(bool animation) {
        estDetruit = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        if (animation == true) {
            for (int i = 0; i < 7; i++) {
                _enfant[i].SetParent(null);
                _enfant[i].gameObject.GetComponent<Rigidbody>().AddForce(0.5f, 0, 0, ForceMode.Impulse);
                _enfant[i].gameObject.GetComponent<Rigidbody>().useGravity = true;
                _enfant[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            StartCoroutine("DetruireGrille", 3);
        }
        else {
            DetruireEnfant();
        }
    }

    private float _pourcentage;
    IEnumerator DetruireGrille(float seconde) {
        _pourcentage = 1 / seconde;
        InvokeRepeating("BaisserOpacite", 0, 1);
        yield return new WaitForSeconds(seconde);
        CancelInvoke();

        DetruireEnfant();
        StopAllCoroutines();
    }

    void DetruireEnfant() {
        for (int i = 0; i < 7; i++) {
            Destroy(_enfant[i].gameObject);
        }
    }

    void BaisserOpacite() {
        for (int i = 0; i < 7; i++) {
            var couleur =_enfant[i].GetComponent<MeshRenderer>().material.color;
            couleur.a -= _pourcentage;
            _enfant[i].GetComponent<MeshRenderer>().material.color = couleur;
        }
    }
}
