using UnityEngine;
using StatsPersoSysteme;
using UnityEngine.SystemeEventsLib;

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
     * Récupère aussi les stats depuis le fichier de sauvegarde, puis actualise l'UI
     * @param void
     * @return void
     */
    void Start () {
        if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
            InfoEvent info = new InfoEvent();

            Sauvegarde _sauvegarde;
            _sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
            string stringJson = System.IO.File.ReadAllText("Assets/Resources/Saves/save.json");
            JsonUtility.FromJsonOverwrite(stringJson, _sauvegarde);

            constitution = _sauvegarde.stats[0];
            force = _sauvegarde.stats[1];
            attaque = _sauvegarde.stats[2];
            chance = _sauvegarde.stats[3];

            info.stats.Constitution = constitution;
            info.stats.Force = force;
            info.stats.Attaque = attaque;
            info.stats.Chance = chance;

            SystemeEvents.Instance.LancerEvent(NomEvent.levelUpEvent, info);
        }
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
        int[] stats = new int[4] { (int)Constitution.Stat, (int)Force.Stat, (int)Attaque.Stat, (int)Chance.Stat };
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