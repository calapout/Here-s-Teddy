using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet l'activation de l'avion NAGAZAKI lors du contact avec sont box collider
/// </summary>
/// <remarks>auteur: Jimmy Tremblay-Bernier</remarks>

public class activationNAGAZAKI : MonoBehaviour {

    public GameObject NAGAZAKI; //référence à l,avion NAGAZAKI

    /// <summary>
    /// active l'avions lors de la colission avec le trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            NAGAZAKI.SetActive(true);
            Destroy(gameObject);
        }
    }

}
