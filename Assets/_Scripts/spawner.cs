using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class spawner : MonoBehaviour {
    public float decalageY;
    public int numeroEnnemi;

    private GameObject _joueur;
    private bool _estMort = true;

    private InfoEvent infoEvennement = new InfoEvent();

    private void Start() {
        _joueur = GameObject.Find("Teddy");
        infoEvennement.Position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageY, 0);
        infoEvennement.Ennemi = numeroEnnemi;
    }

    private void Update() {
        if (Vector3.Distance(_joueur.transform.position, gameObject.transform.position) < 1 && _estMort == true) {
            InstantierEnnemi();
        }
        Debug.Log(Vector3.Distance(_joueur.transform.position, gameObject.transform.position));
    }


    /*private void MortEnnemi(InfoEvent infoEvent) {
        if (infoEvent.Cible == infoEvennement.Cible) {
            _estMort = true;
        }
    }*/

    private void InstantierEnnemi() {
        SystemeEvents.Instance.LancerEvent(NomEvent.spawnEvent, infoEvennement);
        _estMort = false;
    }
}
