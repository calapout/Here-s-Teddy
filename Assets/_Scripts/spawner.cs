using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe gérant les spawners
 * @author Jimmy Tremblay-Bernier
 */
public class spawner : MonoBehaviour {
    //variable publiques
    public float decalageY;
    public bool estMort = true;
    public int ennemiASpawn;

    //variables privées
    private GameObject _joueur;

    private InfoEvent infoEvennement = new InfoEvent();

    //évênnement de départ
    private void Start() {
        _joueur = GameObject.Find("Teddy");
        var transformPosition = gameObject.transform.position;
        infoEvennement.Position = new Vector3(transformPosition.x, transformPosition.y + decalageY, transformPosition.z);
        infoEvennement.Cible = gameObject;
        infoEvennement.Ennemi = ennemiASpawn;
    }
    //boucle de mise à jour
    private void Update() {
        if (Vector3.Distance(_joueur.transform.position, gameObject.transform.position) > 1 && Vector3.Distance(_joueur.transform.position, gameObject.transform.position) < 1.1 && estMort == true) {
            InstantierEnnemi();
        }
    }

    /***
     * lance l'évênement d'instantiation d'un ennemi
     * @param void
     * @return void
     */
    private void InstantierEnnemi() {
        SystemeEvents.Instance.LancerEvent(NomEvent.spawnEvent, infoEvennement);
        estMort = false;
    }
}
