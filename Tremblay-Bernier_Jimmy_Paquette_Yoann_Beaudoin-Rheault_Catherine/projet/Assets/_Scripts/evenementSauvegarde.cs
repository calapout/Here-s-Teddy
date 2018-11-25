using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe permettant de lancer un évênnement de sauvegarde
 * @author Jimmy Tremblay-Bernier
 */
public class evenementSauvegarde : MonoBehaviour {

    //lance un évênnement de sauvegarde lors de la collision avec teddy
    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Teddy") {
            SystemeEvents.Instance.LancerEvent(NomEvent.sauvegardeEvent, new InfoEvent());
        }
    }
}
