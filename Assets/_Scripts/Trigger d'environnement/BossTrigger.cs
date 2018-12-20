using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de gérer l'activation d'un collider empêchant de fuir le boss. Désactive aussi les spawners et les avions
/// </summary>
public class BossTrigger : MonoBehaviour {

    public BoxCollider box; //référence au box collider à activer
    public GameObject poulet; //référence au BOSS

    /// <summary>
    /// Lors de la collision avec le trigger, les ennemis sont désactivés et le collider secondaire est activer
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            box.enabled = true;
            poulet.transform.GetChild(0).GetComponent<Animator>().SetTrigger("actif");
            poulet.GetComponent<DeplacementPoulet>().actif = true;
            Destroy(GameObject.Find("Spawners"));
            Destroy(GameObject.Find("NAGAZAKI"));
            Destroy(GameObject.Find("HIROSHIMA"));
        }
    }
}
