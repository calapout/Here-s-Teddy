using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement
using UnityEngine.SceneManagement;

/**
 * chargerMenu.cs
 * Script qui permet de charger un autre scene
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
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
        //Si la scène actuelle n'est pas celle de fin du jeu...
        if (SceneManager.GetActiveScene().name != "Fin")
        {
            SystemeEvents.Instance.LancerEvent(NomEvent.chargementSceneEvent, infoEvent); //Lancement d'un événement de chargement de scene
        }
        else
        {
            //Sinon, charger la scène du menu principal
            SceneManager.LoadSceneAsync(0);
        }
    }
}
