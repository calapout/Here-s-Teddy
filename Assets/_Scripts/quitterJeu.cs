using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement
using UnityEngine.SceneManagement;

/**
 * quitterJeu.cs
 * Script qui permet au jeu de se fermer
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
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
        //Si la scène actuelle n'est pas celle de fin du jeu...
        if (SceneManager.GetActiveScene().name != "Fin")
        {
            SystemeEvents.Instance.LancerEvent(NomEvent.quitterEvent, infoEvent); //Lancement d'un événement de fermeture du jeu
        }
        else
        {
            //Sinon, fermer le jeu
            Application.Quit();
        }
    }
}
