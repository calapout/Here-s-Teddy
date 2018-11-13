using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement

/**
 * chargerMenu.cs
 * Script qui permet de charger un autre scene
 * @author Yoann Paquette
 * @version Dimanche 11 Novembre 2018
 */
public class chargerMenu : MonoBehaviour {

    InfoEvent infoEvent = new InfoEvent(); //Class d'information d'envoi à un événement

    /**
     * Fonction qui lance un événement de chargement de scene
     * @param void
     * @return void
     */
    public void Charger()
    {
        SystemeEvents.Instance.LancerEvent(NomEvent.chargementEvent, infoEvent); //Lancement d'un événement de chargement de scene
    }
}
