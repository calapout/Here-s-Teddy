﻿using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement

/**
 * quitterJeu.cs
 * Script qui permet au jeu de se fermer
 * @author Yoann Paquette
 * @version Dimanche 11 Novembre 2018
 */
public class quitterJeu : MonoBehaviour {

    InfoEvent infoEvent = new InfoEvent(); //Class d'information d'envoi à un événement

    /**
     * Fonction qui lance un événement de demande de fermeture du jeu
     * @param void
     * @return void
     */
    public void Quitter()
    {
        SystemeEvents.Instance.LancerEvent(NomEvent.quitterEvent, infoEvent); //Lancement d'un événement de fermeture du jeu
    }
}