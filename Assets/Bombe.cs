using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombe : MonoBehaviour {
    public GameObject parent;
    public int degats;

    private void OnCollisionEnter(Collision collision) {
        StartCoroutine("Delai");
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    IEnumerator Delai() {
        yield return new WaitForSeconds(1f);
        Detruire();
    }

    void Detruire() {
        StopAllCoroutines();
        if (parent != null) {
            parent.GetComponent<DeplacementAvion>().estEnAttaque = false;
        }
        Destroy(gameObject);
    }
}
