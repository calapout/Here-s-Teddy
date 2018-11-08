using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class spawner : MonoBehaviour {
    public float decalageY;
    public int numeroEnnemi;
    public bool estMort = true;

    private GameObject _joueur;

    private InfoEvent infoEvennement = new InfoEvent();

    private void Start() {
        _joueur = GameObject.Find("Teddy");
        var transformPosition = gameObject.transform.position;
        infoEvennement.Position = new Vector3(transformPosition.x, transformPosition.y + decalageY, transformPosition.z);
        infoEvennement.Ennemi = numeroEnnemi;
        infoEvennement.Cible = gameObject;
    }

    private void Update() {
        if (Vector3.Distance(_joueur.transform.position, gameObject.transform.position) > 0.75 && Vector3.Distance(_joueur.transform.position, gameObject.transform.position) < 1 && estMort == true) {
            InstantierEnnemi();
        }
    }

    private void InstantierEnnemi() {
        SystemeEvents.Instance.LancerEvent(NomEvent.spawnEvent, infoEvennement);
        estMort = false;
    }
}
