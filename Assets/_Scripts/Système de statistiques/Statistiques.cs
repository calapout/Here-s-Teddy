using UnityEngine;
using StatsPersoSysteme;

public class Statistiques : MonoBehaviour {

    public float constitution = 10f;
    public float force = 10f;
    public float attaque = 0f;
    public float chance = 10f;

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
}
