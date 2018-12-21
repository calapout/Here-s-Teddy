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
    public GameObject panelStats; //Ref du panneau des statistiques du joueur dans le menu pause

    //Couleurs pour l'animation (de la barre) d'utilisation de la rage
    public Color couleurAlerte1;
    public Color couleurAlerte2;

    GameObject barreVie; //Ref de la barre de vie du joueur
    GameObject barreRage; //Ref de la barre de rage du joueur
    GameObject barreExp; //Ref de la barre d'expérience
    GameObject glowRage; //Ref de l'indicateur d'activation de la rage

    GameObject valConsti; //Ref de la valeur de l'affichage de la Constitution dans le menu pause
    GameObject valForce; //Ref de la valeur de l'affichage de la Force dans le menu pause
    GameObject valAttaque; //Ref de la valeur de l'affichage de l'Attaque dans le menu pause
    GameObject valChance; //Ref de la valeur de l'affichage de la Chance dans le menu pause

    bool rageRemplit = false; //Indique si la barre de rage est pleine
    bool alerteEnCours = false; //Indique si le mode Rage est en cours
    float vitesseTransition = 0.05f; //Vitesse de l'animation de l'alerte
    float lerpAlerte = 0f; // 0 à 1

    /**
     * Fonction qui gère les abonnements aux différents événements à la création ou à l'activation du script
     * @param void
     * @return void
     */
    void OnEnable()
    {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargementSceneEvent, ChargementEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiRageEvent, UpdateUiRageEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.levelUpEvent, LevelUpEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.initEvent, InitEvent);
    }

    /**
     * Fonction qui gère les désabonnements aux différents événements à la destruction ou à la désactivation du script
     * @param void
     * @return void
     */
    void OnDisable()
    {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargementSceneEvent, ChargementEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiRageEvent, UpdateUiRageEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.levelUpEvent, LevelUpEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.initEvent, InitEvent);

    }

    /**
     * Fonction d'initialisation des éléments UI du menu pause au chargement initial du script
     * @param void
     * @return void
     */
    void Awake()
    {
        barreVie = panelPrincipal.transform.GetChild(0).gameObject;
        barreRage = panelPrincipal.transform.GetChild(1).gameObject;
        barreExp = panelPrincipal.transform.GetChild(2).gameObject;
        glowRage = panelPrincipal.transform.GetChild(3).gameObject;

        valConsti = panelStats.transform.GetChild(0).GetChild(1).gameObject;
        valForce = panelStats.transform.GetChild(1).GetChild(1).gameObject;
        valAttaque = panelStats.transform.GetChild(2).GetChild(1).gameObject;
        valChance = panelStats.transform.GetChild(3).GetChild(1).gameObject;
    }

    /**
     * Fonction de vérification d'entrée clavier par le joueur
     * @param void
     * @return void
     */
    void Update()
    {
        //Si le joueur appuit sur la touche H, que le mode Rage ne soit pas en cours et que la barre soit remplit...
        if (Input.GetKeyDown(KeyCode.H) && !alerteEnCours && rageRemplit)
        {
            //Lance le mode Rage
            alerteEnCours = true;
            rageRemplit = false;
            InvokeRepeating("AlerteRage", 0, vitesseTransition); //Mode Rage Activer
        }
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
     * Fonction qui est éxécutée lors de la réception d'un événement d'update de la rage du joueur
     * @param class InfoEvent info
     * @return void
     */
    void UpdateUiRageEvent(InfoEvent info)
    {
        UpdateRage(info.Rage, info.RageMax);
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement de montée en niveau du joueur
     * @param class InfoEvent info
     * @return void
     */
    void LevelUpEvent(InfoEvent info)
    {
        UpdateStats(info.stats);
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement dinitialisation de paramètres du jeu
     * @param class InfoEvent info
     * @return void
     */
    void InitEvent(InfoEvent info)
    {
        UpdateVie(info.HP, info.HPMax);
        UpdateRage(info.Rage, info.RageMax);
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
        barreVie.GetComponent<Slider>().value = (float)vie;
        barreVie.GetComponent<Slider>().maxValue = (float)vieMax;
        //Menu Pause - Panneau d'état
        panelEtat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = vie.ToString() + "/" + vieMax.ToString();
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
        barreExp.GetComponent<Slider>().value = (float)exp;
        barreExp.GetComponent<Slider>().maxValue = (float)expMax;
        //Menu Pause - Panneau d'expérience
        panelExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = expTotal.ToString();
        panelExp.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = expNextNiveau.ToString();
        //Menu Pause - Indicateur de niveau
        niveau.GetComponent<TextMeshProUGUI>().text = niv.ToString();
    }

    /**
     * Fonction qui met à jour les statistiques dans le menu pause
     * @param InfoEvent.Stats stats (Objet de statistiques)
     * @return void
     */
    void UpdateStats(InfoEvent.Stats stats)
    {
        valConsti.GetComponent<TextMeshProUGUI>().text = stats.Constitution.ToString();
        valForce.GetComponent<TextMeshProUGUI>().text = stats.Force.ToString();
        valAttaque.GetComponent<TextMeshProUGUI>().text = stats.Attaque.ToString();
        valChance.GetComponent<TextMeshProUGUI>().text = stats.Chance.ToString();
    }

    /**
     * Fonction qui met à jour la rage du joueur dans le UI
     * @param int vie (Vie du joueur)
     * @param int vieMax (Vie max du joueur)
     * @return void
     */
    void UpdateRage(int rage, int rageMax)
    {
        //Panneau principal
        barreRage.GetComponent<Slider>().value = (float)rage;
        barreRage.GetComponent<Slider>().maxValue = (float)rageMax;
        //Menu Pause - Panneau d'état
        panelEtat.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = rage.ToString() + "/" + rageMax.ToString();

        //Si la rage dépasse ou atteint la quantité maximal...
        if (rage >= rageMax)
        {
            //Indique quelle est pleine
            rageRemplit = true;
            glowRage.GetComponent<Image>().color = Color.white;
        }

        //Si la rage atteint 0...
        if (rage == 0)
        {
            //Indique quelle est vide
            glowRage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            alerteEnCours = false;
            CancelInvoke("AlerteRage"); //Mode Rage Désactiver
        }
    }

    /**
     * Fonction de contrôle l'animation (alerte) du mode rage
     * @param void
     * @return void
     */
    void AlerteRage()
    {
        glowRage.GetComponent<Image>().color = Color.Lerp(couleurAlerte1, couleurAlerte2, lerpAlerte);
        lerpAlerte += vitesseTransition;

        //Si le blend de la seconde couleur atteint 100%...
        if (lerpAlerte > 1f)
        {
            //Reset à la prmière couleur
            lerpAlerte = 0f;
        }
    }
}