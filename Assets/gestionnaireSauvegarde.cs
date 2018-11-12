using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class gestionnaireSauvegarde : MonoBehaviour {
    /**liste de chose à sauvegarder
     * Position du joueur
     * Points de vie du joueur
     * Inventaire du joueur
     * Arme du joueur
     * *a lot of things I guess*
     * 
     * 
     * 
     * 
     * 
     * 
     */

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.sauvegardeEvent, sauvegarder);
    }

    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.sauvegardeEvent, sauvegarder);
    }



    void sauvegarder(InfoEvent evennement) {

    }
}
