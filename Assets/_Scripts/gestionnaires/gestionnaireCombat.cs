using UnityEngine;
using UnityEngine.SystemeEventsLib;

/**
 * gestionnaireCombat.cs
 * Classe controlant les mouvements du personnage, ses points de vies et son inventaire.
 * @author Jimmy Tremblay-Bernier
 * @author Yoann paquette
 * @version Mercredi 19 Décembre 2018
 */
public class gestionnaireCombat : MonoBehaviour {
    // variables publiques
    public int experience; //Expérience actuelle
    public int experienceMax = 10; //Expérience a atteindre avant le prochain niveau
    public int niveau = 1; //Niveau actuel
    public GameObject perso; //Ref au joueur
    Rage rageRef; //Ref au script de la rage

    int vieMaxBase; //Vie maximum initialle

    public int experienceTotal = 0; //expérience totale acquise
    public int experienceNextLevel = 10; //expérience nécessaire au level up

    //Activation des fonctions de débugage dans la console 
    [Header("Debug")]
    public bool mortEnnemi;
    public bool exp;
    public bool levelUp;
    public bool stats;

    // variables privée
    InfoEvent infoEvent2 = new InfoEvent();

    /**
     * Fonction qui gère les abonnements aux différents événements à la création ou à l'activation du script
     * @param void
     * @return void
     */
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, MortEnnemiEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.initEvent, InitEvent);
    }

    /**
     * Fonction qui gère les désabonnements aux différents événements à la destruction ou à la désactivation du script
     * @param void
     * @return void
     */
    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiEvent, MortEnnemiEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.initEvent, InitEvent);
    }

    // évênnement de départ
    void Start() {
        rageRef = perso.GetComponent<Rage>();
        vieMaxBase = perso.GetComponent<joueur>().pointDeVieMax;

        infoEvent2.ExpMax = experienceMax;
        infoEvent2.ExpNextNiveau = experienceMax;
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement de mort d'un ennemi
     * @param class InfoEvent infoEvent
     * @return void
     */
    void MortEnnemiEvent(InfoEvent infoEvent)
    {
        //Gain en EXP
        AjouterExp(infoEvent);
        //Update de la rage
        UpdateRage();
    }

    /**
     * Fonction qui est éxécutée lors de la réception d'un événement d'initialisation de paramètres du jeu
     * @param class InfoEvent infoEvent
     * @return void
     */
    void InitEvent(InfoEvent infoEvent)
    {
        perso = infoEvent.Cible;
    }

    /***
     * gère l'ajout des points d'expériences, lances des évênnements pour l'affichage et permet la monter de niveau
     * @param InfoEvent [paramètre contenant toutes les informations nécéssaire comme le nombre de point d'expérience à ajouter]
     * @return void
     */
    void AjouterExp(InfoEvent infoEvent) {
        MortEnnemiDebug(infoEvent);

        experience += infoEvent.Experience;
        experienceTotal += infoEvent.Experience;
        infoEvent2.ExpTotal = experienceTotal;
        ExpDebug();

        //Si l'expérience atteint ou dépasse le maximum...
        if (experience - experienceMax >= 0) {
            //Montée en niveau
            NiveauSuperieur();
        }
        infoEvent2.Experience = experience;
        infoEvent2.ExpMax = experienceMax;
        infoEvent2.Niveau = niveau;
        infoEvent2.ExpNextNiveau = experienceNextLevel;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiExpEvent, infoEvent2);
    }

    /***
     * gère la monté de niveaux
     * @param void
     * @return void
     */
    void NiveauSuperieur() {
        experience = experience - experienceMax;
        experienceMax = (int)(experienceMax * 1.2);
        experienceNextLevel += experienceMax;
        infoEvent2.ExpNextNiveau += experienceMax;

        niveau++;
        LevelUpDebug();

        //Si l'expérience dépasse une fois supplémentaire le maximum...
        if (experience > experienceMax) {
            //Montée en niveau
            NiveauSuperieur();
        }
        //Augmentation des stats
        AugmenterStats();
    }

    /**
     * Fonction qui gère l'augmentation des statistiques et de ce qu'elles modifies lors d'une montée en niveau
     * @param void
     * @return void
     */
    void AugmenterStats()
    {
        int points = Random.Range(2, 5); //Nombre de points de stats générés

        //Boucle à travers chaque points qui à été générés
        for (int i = 0, rng; i < points; i++)
        {
            rng = Random.Range(0, 3); //Choix aléatoire de la stat à améliorer
            //Application du modificateur de montée en niveau selon le chiffre généré
            switch (rng)
            {
                case 0:
                    perso.GetComponent<Statistiques>().Constitution.AjouterModif(new StatsPersoSysteme.ModifStat(1, "LevelUp"));
                    break;
                case 1:
                    perso.GetComponent<Statistiques>().Force.AjouterModif(new StatsPersoSysteme.ModifStat(1, "LevelUp"));
                    break;
                case 2:
                    perso.GetComponent<Statistiques>().Chance.AjouterModif(new StatsPersoSysteme.ModifStat(1, "LevelUp"));
                    break;
            }
        }

        infoEvent2.stats.Constitution = perso.GetComponent<Statistiques>().Constitution.Stat;
        infoEvent2.stats.Force = perso.GetComponent<Statistiques>().Force.Stat;
        infoEvent2.stats.Chance = perso.GetComponent<Statistiques>().Chance.Stat;

        //Augmentation de l'attaque selon la force
        AugmenterAttaque();

        SystemeEvents.Instance.LancerEvent(NomEvent.levelUpEvent, infoEvent2); //Lancement d'un événement de montée en niveau
        StatsDebug();

        //Augmentation de la vie maximale selon la constitution
        AugmenterVieMax();
    }

    /**
     * Fonction qui gère l'augmentation de la vie maximale selon la Constitution
     * @param void
     * @return void
     */
    void AugmenterVieMax()
    {
        perso.GetComponent<joueur>().pointDeVieMax = vieMaxBase + VieMaxParPtsConsti(infoEvent2.stats.Constitution);
        infoEvent2.HP = perso.GetComponent<joueur>().pointDeVie;
        infoEvent2.HPMax = perso.GetComponent<joueur>().pointDeVieMax;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent2); //Lancement d'un événement d'update de la vie dans le UI
    }

    /**
     * Fonction qui calcule et retourne le nombre de points de vie maximale selon la Constitution
     * @param float consti (Valeur de la Constitution)
     * @return int (Nouvelle valeur du maximum de points de vie)
     */
    int VieMaxParPtsConsti(float consti)
    {
        int vieMax = 0;
        //Boucle à travers la quantité de points de constitution
        for (int i = 0; i <= consti - perso.GetComponent<Statistiques>().constitution; i++)
        {
            if (i != 0 && i % 2 == 0)
            {
                vieMax++;
            }
        }
        return vieMax;
    }

    /**
     * Fonction qui gère l'augmentation de l'Attaque selon la force
     * @param void
     * @return void
     */
    void AugmenterAttaque()
    {
        perso.GetComponent<Statistiques>().Attaque.RetirerTousModifSource("Force");
        perso.GetComponent<Statistiques>().Attaque.AjouterModif(new StatsPersoSysteme.ModifStat(AttaqueParPtsForce(infoEvent2.stats.Force), "Force"));
        infoEvent2.stats.Attaque = perso.GetComponent<Statistiques>().Attaque.Stat;
    }

    /**
     * Fonction qui calcule et retourne le nombre de points d'Attaque selon la Force
     * @param float force (Valeur de la Force)
     * @return int (Nouvelle valeur d'Attaque)
     */
    int AttaqueParPtsForce(float force)
    {
        int attaque = 0;
        //Boucle à travers la quantité de points de force
        for (int i = 0; i <= force - perso.GetComponent<Statistiques>().force; i++)
        {
            if (i != 0 && i % 5 == 0)
            {
                attaque++;
            }
        }
        return attaque;
    }

    /**
     * Fonction qui update la quantité de rage si elle provient de la mort d'un ennemi
     * @param void
     * @return void
     */
    void UpdateRage()
    {
        rageRef.pointsDeRage = rageRef.GainRage(Rage.TypeGain.Kill);
        rageRef.RageEventSetup(); //Configuration initialle de l'event d'update de la rage
    }

    /**
     * Fonction de débugage de la mort d'un ennemi
     * @param void
     * @return void
     */
    void MortEnnemiDebug(InfoEvent info)
    {
        if (mortEnnemi)
        {
            Debug.Log("ENNEMIE MORT: " + info.Experience);
        }
    }

    /**
     * Fonction de débugage de la réception d'EXP
     * @param void
     * @return void
     */
    void ExpDebug()
    {
        if (exp)
        {
            Debug.Log("EXPERIENCE ACTUELLE : " + experience);
        }
    }

    /**
     * Fonction de débugage de la montée en niveau
     * @param void
     * @return void
     */
    void LevelUpDebug()
    {
        if (levelUp)
        {
            Debug.Log("LEVEL UP: " + niveau);
        }
    }

    /**
     * Fonction de débugage de l'augmentation des stats
     * @param void
     * @return void
     */
    void StatsDebug()
    {
        if (stats)
        {
            Debug.Log("STATISTIQUES:\n\n" +
                "     <color=green>CONST:" + perso.GetComponent<Statistiques>().Constitution.Stat + "</color>\n" +
                "     <color=yellow>FORCE:" + perso.GetComponent<Statistiques>().Force.Stat + "</color>\n" +
                "     <color=red>ATTAQUE:" + perso.GetComponent<Statistiques>().Attaque.Stat + "</color>\n" +
                "     <color=blue>CHANCE:" + perso.GetComponent<Statistiques>().Chance.Stat + "</color>");
        }
    }
}