using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe gérant le spawn d'un ennemi lors de la réception d'évênnements
 * @author Jimmy Tremblay-Bernier
 */

public class gestionnaireSpawn : MonoBehaviour {
    public GameObject[] ennemieASpawn = new GameObject[2];

    // Evênnements d'activation
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.spawnEvent, InstantierEnnemi);
    }

    // Evênnements de désactivation
    private void OnDisable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.spawnEvent, InstantierEnnemi);
    }

    /***
     * instancie un ennemi grâce au paramètre infoEvent
     * @param InfoEvent [contient l'ensemble des paramètres nécéssaire à l'instantiation des ennemis]
     * @return void
     */
    private void InstantierEnnemi(InfoEvent infoEvent) {
        var clone = Instantiate(ennemieASpawn[infoEvent.Ennemi]);
        clone.transform.position = infoEvent.Position;
        clone.GetComponent<Ennemi>().spawner = infoEvent.Cible;
    }
}
