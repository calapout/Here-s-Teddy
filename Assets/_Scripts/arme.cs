using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arme : MonoBehaviour {
    public float tempDesactivation;
    public int degats;

    private void OnEnable() {
        StartCoroutine("Desactiver", 0.5f);
    }

    IEnumerator Desactiver(float secondes) {
        yield return new WaitForSeconds(secondes);
        Detruire();
    }

    void Detruire() {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
