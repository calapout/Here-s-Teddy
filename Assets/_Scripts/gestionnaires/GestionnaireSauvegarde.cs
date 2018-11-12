using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class GestionnaireSauvegarde : MonoBehaviour {

    private GameObject _teddy;
    private joueur _teddyScript;
    private gestionnaireCombat _gestionnaireCombat;

    private void Start() {
        _teddy = GameObject.Find("Teddy");
        _teddyScript = _teddy.GetComponent<joueur>();
        _gestionnaireCombat = GameObject.Find("Manager jeu").GetComponent<gestionnaireCombat>();
    }

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargerEvent, Charger);
    }

    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargerEvent, Charger);
    }

    void Sauvegarder(InfoEvent evennement) {
        Debug.Log("ici");
        Sauvegarde sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
        Debug.Log(sauvegarde);
        sauvegarde.positionJoueur = _teddy.transform.position;
        sauvegarde.pointVieJoueur = _teddyScript.pointDeVie;
        sauvegarde.pointVieMaxJoueur = _teddyScript.pointDeVieMax;
        sauvegarde.inventaireJoueur = _teddyScript.inventaireObjet;
        sauvegarde.inventaireJoueurQte = _teddyScript.inventaireObjetQte;
        sauvegarde.armeEquipe = _teddyScript.arme.name;
        //sauvegarde.inventaireArme;
        sauvegarde.experienceJoueur = _gestionnaireCombat.experience;
        sauvegarde.experienceMaxJoueur = _gestionnaireCombat.experienceMax;
        sauvegarde.niveau = _gestionnaireCombat.niveau;
        //sauvegarde.stats;
        string stringJson = JsonUtility.ToJson(sauvegarde);
        Debug.Log(stringJson);
    }

    void Charger(InfoEvent evennement) {

    }
}
