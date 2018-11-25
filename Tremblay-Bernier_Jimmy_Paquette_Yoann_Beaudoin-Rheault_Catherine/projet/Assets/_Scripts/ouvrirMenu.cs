using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement
using UnityEngine.UI;

/**
 * ouvrirMenu.cs
 * Script qui permet de modifier l'affichage des différentes sections du UI
 * @author Yoann Paquette
 * @version Lundi 12 Novembre 2018
 */
public class ouvrirMenu : MonoBehaviour {

    public GameObject menuPause; //Ref du menu de pause
    public GameObject panelNav; //Ref du panneau de navigation du menu pause
    public GameObject panelArmes; //Ref du panneau d'inventaire des armes
    Button[] btnsNav; //Tableau de ref de tous les emplacements d'armes
    InfoEvent infoEvent = new InfoEvent(); //Class d'information d'envoi à un événement

    void Start ()
    {
        infoEvent.EnPause = false;
        btnsNav = panelNav.transform.GetComponentsInChildren<Button>();
    }

    /**
     * Fonction qui vérifie à chaque frame si le joueur a appuyé sur la touche Escape
     * @param void
     * @return void
     */
    void Update ()
    {
        //Si la touche Escape est enfoncé...
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Si le panneau des armes est ouvert...
            if (panelArmes.activeSelf)
            {
                //Fermer celui-ci
                OuvrirPanelArmes();
            }
            else
            {
                //Sinon, ouverture ou fermeture du menu pause
                OuvrirMenuPause();
            }
        }
    }

    /**
     * Fonction qui change l'état de la pause et lance un événement pour celle-ci
     * @param void
     * @return void
     */
    void Resume()
    {
        infoEvent.EnPause = !infoEvent.EnPause;
        SystemeEvents.Instance.LancerEvent(NomEvent.pauseEvent, infoEvent); //Lancement d'un événement de pause du jeu
    }

    /**
     * Fonction qui cible le menu pause et qui change son état
     * @param void
     * @return void
     */
    public void OuvrirMenuPause()
    {
        infoEvent.Cible = menuPause;
        Resume();
    }

    /**
     * Fonction qui change l'état du panneau d'armes et des paramètres du menu pause
     * @param void
     * @return void
     */
    public void OuvrirPanelArmes()
    {
        panelArmes.SetActive(!panelArmes.activeSelf);
        //Si le panneau d'armes est ouvert...
        if (panelArmes.activeSelf)
        {
            //Désactiver la navigation du menu pause
            foreach(Button btn in btnsNav)
            {
                btn.interactable = false;
            }
        }
        else
        {
            //Sinon, activer la navigation du menu pause
            foreach (Button btn in btnsNav)
            {
                btn.interactable = true;
            }
        }
    }
}
