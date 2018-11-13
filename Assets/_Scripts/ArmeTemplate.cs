using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * ScriptableObject pour créer et gérer les armes
 * @author Jimmy Tremblay-Bernier
 */
[CreateAssetMenu(fileName = "Nouvelle arme", menuName = "Arme")]
public class ArmeTemplate : ScriptableObject {
    public string nom;
    public int degat;
    public GameObject objet;
}
