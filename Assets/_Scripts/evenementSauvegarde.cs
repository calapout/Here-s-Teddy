using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;
public class evenementSauvegarde : MonoBehaviour {

    private void OnTriggerEnter(Collider collision) {
        Debug.Log("contact");
        if (collision.gameObject.name == "Teddy") {
            SystemeEvents.Instance.LancerEvent(NomEvent.sauvegardeEvent, new InfoEvent());
        }
    }
}
