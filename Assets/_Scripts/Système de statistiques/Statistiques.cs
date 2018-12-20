using UnityEngine;
using StatsPersoSysteme;

/**
 * Statistiques.cs
 * Script d'enregistrement des statistiques du joueur
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class Statistiques : MonoBehaviour {

    //Statistiques par défaut du joueur
    public int constitution = 10;
    public int force = 10;
    public int attaque = 0;
    public int chance = 10;

    //Statistiques actuelles du joueur
    [HideInInspector]
    public StatPerso Constitution;
    [HideInInspector]
    public StatPerso Force;
    [HideInInspector]
    public StatPerso Attaque;
    [HideInInspector]
    public StatPerso Chance;

    /**
     * Fonction d'initialisation de départ des statistiques
     * @param void
     * @return void
     */
    void Start () {
        Constitution.ValeurBase = constitution;
        Force.ValeurBase = force;
        Attaque.ValeurBase = attaque;
        Chance.ValeurBase = chance;
    }

    /**
     * Fonction de récupération des statistiques par défaut dans un tableau
     * @param void
     * @return int[] (Tableau de statistiques)
     */
    public int[] RecupererStat() {
        int[] stats = new int[] { constitution, force, attaque, chance };
        return stats;
    }

    /**
     * Fonction d'assignation des statistiques par défaut depuis un tableau
     * @param int[] stats (Tableau de statistiques)
     * @return void
     */
    public void AssignerStats(int[] stats) {
        this.constitution = stats[0];
        this.force = stats[1];
        this.attaque = stats[2];
        this.chance = stats[3];
    }
}