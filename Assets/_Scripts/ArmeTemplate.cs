using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nouvelle arme", menuName = "Arme")]
public class ArmeTemplate : ScriptableObject {
    public string nom;
    public int degat;
    public GameObject objet;
}
