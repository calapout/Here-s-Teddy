using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class gestionnaireSpawn : MonoBehaviour {
    public GameObject[] ennemieASpawn;

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.spawnEvent, InstantierEnnemi);
    }

    private void OnDisable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.spawnEvent, InstantierEnnemi);
    }


    private void InstantierEnnemi(InfoEvent infoEvent) {
        var clone = Instantiate(ennemieASpawn[infoEvent.Ennemi]);
        clone.transform.position = infoEvent.Position;
        clone.GetComponent<ennemi>().spawner = infoEvent.Cible;
    }
}
