using UnityEngine;
using UnityEngine.SystemeEventsLib; //Namespace du système d'événement
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; //Namespace pour TextMeshPro

/**
 * gestionnaireUi.cs
 * Script de gestion de l'update du UI lors de la réception de certain événements
 * @author Yoann Paquette
 * @version Lundi 12 Novembre 2018
 */
public class gestionnaireUi : MonoBehaviour {

    public GameObject panelPrincipal; //Ref du panneau principal du UI (Vie, Rage, Exp)
    public GameObject panelEtat; //Ref du panneau d'état du joueur dans le menu pause
    public GameObject panelExp; //Ref du panneau d'expérience dans le menu pause
    public GameObject niveau; //Ref de l'indicateur de niveau du joueur dans le menu pause
    GameObject barreVie; //Ref de la barre de vie du joueur
    GameObject barreExp; //Ref de la barre d'expérience

    /**
     * Fonction qui gère les abonnements aux différents événements à la création ou à l'activation du script
     * @param void
     * @return void
     */
    void OnEnable()
    {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
    }

    /**
     * Fonction qui gère les désabonnements aux différents événements à la destruction ou à la désactivation du script
     * @param void
     * @return void
     */
    void OnDisable()
    {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
    }

    void Start()
    {
        barreVie = panelPrincipal.transform.GetChild(0).gameObject;
        barreExp = panelPrincipal.transform.GetChild(2).gameObject;
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement de pause du jeu
     * @param class InfoEvent info
     * @return void
     */
    void PauseEvent(InfoEvent info)
    {
        //Si le jeu doit être en pause...
        if (info.EnPause)
        {
            //Activer la pause
            Pause(info.Cible);
        }
        else
        {
            //Sinon, remettre le jeu en marche
            Resume(info.Cible);
        }
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement de chargement d'une scène
     * @param class InfoEvent info
     * @return void
     */
    void ChargementEvent(InfoEvent info)
    {
        ChargerMenu();
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement pour quitter le jeu
     * @param class InfoEvent info
     * @return void
     */
    void QuitterEvent(InfoEvent info)
    {
        QuitterJeu();
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement d'update de la vie du joueur
     * @param class InfoEvent info
     * @return void
     */
    void UpdateUiVieEvent(InfoEvent info)
    {
        UpdateVie(info.HP, info.HPMax);
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement d'update de l'expérience du joueur
     * @param class InfoEvent info
     * @return void
     */
    void UpdateUiExpEvent(InfoEvent info)
    {
        UpdateExp(info.Experience, info.ExpMax, info.ExpTotal, info.ExpNextNiveau, info.Niveau);
    }

    /**
     * Fonction qui active le menu et qui met la pause
     * @param GameObject ui (Cible qui doit être activer)
     * @return void
     */
    void Pause(GameObject ui)
    {
        ui.SetActive(true);
        Time.timeScale = 0f; //Changer le scale du temps du jeu à 0 permet de mettre le jeu en pause
    }

    /**
     * Fonction qui désactive le menu et qui résume le jeu
     * @param GameObject ui (Cible qui doit être désactiver)
     * @return void
     */
    void Resume(GameObject ui)
    {
        ui.SetActive(false);
        Time.timeScale = 1f; //Et le mettre à 1 permet de résumer la partie
    }

    /**
     * Fonction de chargement de la scène du menu principal
     * @param void
     * @return void
     */
    void ChargerMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    /**
     * Fonction qui permet de quitter le jeu
     * @param void
     * @return void
     */
    void QuitterJeu()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }

    /**
     * Fonction qui met à jour la vie du joueur dans le UI
     * @param int vie (Vie du joueur)
     * @param int vieMax (Vie max du joueur)
     * @return void
     */
    void UpdateVie(int vie, int vieMax)
    {
        //Panneau principal
        barreVie.GetComponent<Slider>().value = vie;
        barreVie.GetComponent<Slider>().maxValue = vieMax;
        //Menu Pause - Panneau d'état
        panelEtat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = vie.ToString() + "/" + vieMax;
    }

    /**
     * Fonction qui met à jour l'expérience du joueur dans le UI
     * @param int exp (Expérience du joueur)
     * @param int expMax (Expérience pour monter de niveau)
     * @param int expTotal (Expérience total du joueur)
     * @param int expNextNiveau (Expérience total pour atteindre le prochain niveau)
     * @param int niv (Niveau du joueur)
     * @return void
     */
    void UpdateExp(int exp, int expMax, int expTotal, int expNextNiveau, int niv)
    {
        //Panneau principal
        barreExp.GetComponent<Slider>().value = exp;
        barreExp.GetComponent<Slider>().maxValue = expMax;
        //Menu Pause - Panneau d'expérience
        panelExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = expTotal.ToString();
        panelExp.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = expNextNiveau.ToString();
        //Menu Pause - Indicateur de niveau
        niveau.GetComponent<TextMeshProUGUI>().text = niv.ToString();
    }
}