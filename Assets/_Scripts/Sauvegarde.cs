using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;


[System.Serializable]
public class Sauvegarde : ScriptableObject {
    /**liste de chose à sauvegarder
     * Position du joueur
     * Points de vie du joueur
     * Inventaire du joueur
     * Arme du joueur
     * Experience du joueur
     * Si le boss est mort
     * Stats
     */
    public Vector3 positionJoueur;
    public int pointVieJoueur;
    public int pointVieMaxJoueur;
    public List<string> inventaireJoueur = new List<string>();
    public List<int> inventaireJoueurQte = new List<int>();
    public string armeEquipe;
    public List<string> inventaireArme = new List<string>();
    public int experienceJoueur;
    public int experienceMaxJoueur;
    public int[] stats = new int[4];
    public int niveau;
}
