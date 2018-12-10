using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe controlant les mouvements du personnage, ses points de vies et son inventaire.
 * @author Jimmy Tremblay-Bernier
 * @author Yoann paquette
 */

public class gestionnaireCombat : MonoBehaviour {
    // variables publiques
    public int experience;
    public int experienceMax = 10;
    public int niveau = 1;
    GameObject perso;
    Rage rageRef;

    int vieMaxBase;

    [Header("Debug")]
    public bool mortEnnemi;
    public bool exp;
    public bool levelUp;
    public bool stats;

    // variables privée
    InfoEvent infoEvent2 = new InfoEvent();

    // évênnement d'activation
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, MortEnnemiEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.initEvent, InitEvent);
    }

    // évênnement de désactivation
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

    void MortEnnemiEvent(InfoEvent infoEvent)
    {
        AjouterExp(infoEvent);
        UpdateRage();
    }

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
        //Debug.Log("ENNEMIE MORT: " + infoEvent.Experience);
        experience += infoEvent.Experience;
        infoEvent2.ExpTotal += infoEvent.Experience;
        ExpDebug();
        //Debug.Log("EXPERIENCE ACTUELLE : " + experience);
        if (experience - experienceMax >= 0) {
            NiveauSuperieur();
        }
        infoEvent2.Experience = experience;
        infoEvent2.ExpMax = experienceMax;
        infoEvent2.Niveau = niveau;
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
        infoEvent2.ExpNextNiveau += experienceMax;
        niveau++;
        LevelUpDebug();
        //Debug.Log("LEVEL UP: " + niveau);
        if (experience > experienceMax) {
            NiveauSuperieur();
        }
        AugmenterStats();
    }

    void AugmenterStats()
    {
        int points = Random.Range(2, 5);

        for (int i = 0, rng; i < points; i++)
        {
            rng = Random.Range(0, 3);
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

        AugmenterAttaque();

        SystemeEvents.Instance.LancerEvent(NomEvent.levelUpEvent, infoEvent2);
        StatsDebug();

        AugmenterVieMax();
    }

    void AugmenterVieMax()
    {
        perso.GetComponent<joueur>().pointDeVieMax = vieMaxBase + VieMaxParPtsConsti(infoEvent2.stats.Constitution);
        infoEvent2.HP = perso.GetComponent<joueur>().pointDeVie;
        infoEvent2.HPMax = perso.GetComponent<joueur>().pointDeVieMax;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiVieEvent, infoEvent2);
    }

    int VieMaxParPtsConsti(float consti)
    {
        int vieMax = 0;
        for (int i = 0; i <= consti - perso.GetComponent<Statistiques>().constitution; i++)
        {
            if (i != 0 && i % 2 == 0)
            {
                vieMax++;
            }
        }
        return vieMax;
    }

    void AugmenterAttaque()
    {
        perso.GetComponent<Statistiques>().Attaque.RetirerTousModifSource("Force");
        perso.GetComponent<Statistiques>().Attaque.AjouterModif(new StatsPersoSysteme.ModifStat(AttaqueParPtsForce(infoEvent2.stats.Force), "Force"));
        infoEvent2.stats.Attaque = perso.GetComponent<Statistiques>().Attaque.Stat;
    }

    int AttaqueParPtsForce(float force)
    {
        int attaque = 0;
        for (int i = 0; i <= force - perso.GetComponent<Statistiques>().force; i++)
        {
            if (i != 0 && i % 5 == 0)
            {
                attaque++;
            }
        }
        return attaque;
    }

    void UpdateRage()
    {
        rageRef.pointsDeRage = rageRef.GainRage(Rage.TypeGain.Kill);
        rageRef.RageEventSetup();
    }

    void MortEnnemiDebug(InfoEvent info)
    {
        if (mortEnnemi)
        {
            Debug.Log("ENNEMIE MORT: " + info.Experience);
        }
    }

    void ExpDebug()
    {
        if (exp)
        {
            Debug.Log("EXPERIENCE ACTUELLE : " + experience);
        }
    }

    void LevelUpDebug()
    {
        if (levelUp)
        {
            Debug.Log("LEVEL UP: " + niveau);
        }
    }

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
