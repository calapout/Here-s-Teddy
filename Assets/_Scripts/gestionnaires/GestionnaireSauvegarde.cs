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
    private conduitScript _ventilation;

    // Evênnements de départ
    private void Start() {
        _teddy = GameObject.Find("Teddy");
        _teddyScript = _teddy.GetComponent<joueur>();
        _statScript = _teddy.GetComponent<Statistiques>();
        _gestionnaireCombat = GameObject.Find("Manager jeu").GetComponent<gestionnaireCombat>();
        _ventilation = GameObject.Find("Conduit.001").GetComponent<conduitScript>();
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
        Sauvegarde sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
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
        sauvegarde.ventilationEstBriser = _ventilation.estDetruit;
        string stringJson = JsonUtility.ToJson(sauvegarde);
        if (System.IO.Directory.Exists("Assets/Resources/Saves")) {
            System.IO.File.WriteAllText("Assets/Resources/Saves/save.json", stringJson);
        }
        else {
            System.IO.Directory.CreateDirectory("Assets/Resources/Saves");
            System.IO.File.WriteAllText("Assets/Resources/Saves/save.json", stringJson);
        }
    }

    /***
     * Gère la conversion du json en gameObject
     * @param InfoEvent [evennements reçu]
     * @return void
     */
    void Charger(InfoEvent evennement) {
        Debug.Log(System.IO.File.Exists("Assets/Resources/Saves/save.json"));
        if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
            
            Debug.Log("LOADING !!!!");
            string stringJson = System.IO.File.ReadAllText("Assets/Resources/Saves/save.json");
            Sauvegarde sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
            JsonUtility.FromJsonOverwrite(stringJson, sauvegarde);
            Debug.Log(sauvegarde);

            _teddy.transform.position = sauvegarde.positionJoueur;
            _teddyScript.pointDeVie = sauvegarde.pointVieJoueur;
            _teddyScript.pointDeVieMax = sauvegarde.pointVieMaxJoueur;
            _teddyScript.inventaireObjet = sauvegarde.inventaireJoueur;
            _teddyScript.inventaireObjetQte = sauvegarde.inventaireJoueurQte;
            _teddyScript.armeActuelle.nom = sauvegarde.armeEquipe;
            _teddyScript.inventaireArme = sauvegarde.inventaireArme;
            _gestionnaireCombat.experience = sauvegarde.experienceJoueur;
            _gestionnaireCombat.experienceMax = sauvegarde.experienceMaxJoueur;
            _gestionnaireCombat.niveau = sauvegarde.niveau;
            _statScript.AssignerStats(sauvegarde.stats);
            if (sauvegarde.ventilationEstBriser == true) {
                _ventilation.GererDetruireGrille(false);
            }
        }
    }
}
