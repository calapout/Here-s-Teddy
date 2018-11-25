using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe qui gère la sauvegarde et le chargement dans un fichier json
 * @author Jimmy Tremblay-Bernier
 */
public class GestionnaireSauvegarde : MonoBehaviour {

    private GameObject _teddy;
    private joueur _teddyScript;
    private Statistiques _statScript;
    private gestionnaireCombat _gestionnaireCombat;

    // Evênnements de départ
    private void Start() {
        _teddy = GameObject.Find("Teddy");
        _teddyScript = _teddy.GetComponent<joueur>();
        _statScript = _teddy.GetComponent<Statistiques>();
        _gestionnaireCombat = GameObject.Find("Manager jeu").GetComponent<gestionnaireCombat>();
    }

    //Evennement lors de l'activation
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargerEvent, Charger);
    }

    //Evennement lors de la désactivation
    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargerEvent, Charger);
    }

    /***
     * Gère la conversion et la sauvegarde dans un fichier json
     * @param InfoEvent [evennements reçu]
     * @return void
     */
    void Sauvegarder(InfoEvent evennement) {
        Debug.Log("ici");
        Sauvegarde sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
        Debug.Log(sauvegarde);
        sauvegarde.positionJoueur = _teddy.transform.position;
        sauvegarde.pointVieJoueur = _teddyScript.pointDeVie;
        sauvegarde.pointVieMaxJoueur = _teddyScript.pointDeVieMax;
        sauvegarde.inventaireJoueur = _teddyScript.inventaireObjet;
        sauvegarde.inventaireJoueurQte = _teddyScript.inventaireObjetQte;
        sauvegarde.armeEquipe = _teddyScript.armeActuelle.nom;
        sauvegarde.inventaireArme = _teddyScript.inventaireArme;
        sauvegarde.experienceJoueur = _gestionnaireCombat.experience;
        sauvegarde.experienceMaxJoueur = _gestionnaireCombat.experienceMax;
        sauvegarde.niveau = _gestionnaireCombat.niveau;
        sauvegarde.stats = _statScript.RecupererStat();
        string stringJson = JsonUtility.ToJson(sauvegarde);
        Debug.Log(stringJson);
        System.IO.File.WriteAllText("Assets/Resources/Saves/save.json", stringJson);
    }

    /***
     * Gère la conversion du json en gameObject
     * @param InfoEvent [evennements reçu]
     * @return void
     */
    void Charger(InfoEvent evennement) {

    }
}
