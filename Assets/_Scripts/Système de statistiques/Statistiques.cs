using UnityEngine;
using StatsPersoSysteme;

public class Statistiques : MonoBehaviour {

    public int constitution = 10;
    public int force = 10;
    public int attaque = 0;
    public int chance = 10;

    [HideInInspector]
    public StatPerso Constitution;
    [HideInInspector]
    public StatPerso Force;
    [HideInInspector]
    public StatPerso Attaque;
    [HideInInspector]
    public StatPerso Chance;

    void Start () {
        Constitution.ValeurBase = constitution;
        Force.ValeurBase = force;
        Attaque.ValeurBase = attaque;
        Chance.ValeurBase = chance;
    }

    public int[] RecupererStat() {
        int[] stats = new int[] { constitution, force, attaque, chance };
        return stats;
    }
}
