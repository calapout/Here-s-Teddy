using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name != "Fin")
        {
            SystemeEvents.Instance.LancerEvent(NomEvent.chargementSceneEvent, infoEvent); //Lancement d'un événement de chargement de scene
        }
        else
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
