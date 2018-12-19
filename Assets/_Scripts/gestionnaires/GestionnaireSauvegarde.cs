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
    private Sauvegarde _sauvegarde;

    // Evênnements de départ
    private void Start() {
        _teddy = GameObject.Find("Teddy");
        _teddyScript = _teddy.GetComponent<joueur>();
        _statScript = _teddy.GetComponent<Statistiques>();
        _gestionnaireCombat = GameObject.Find("Manager jeu").GetComponent<gestionnaireCombat>();
        _ventilation = GameObject.Find("Conduit.001").GetComponent<conduitScript>();
        _sauvegarde = ScriptableObject.CreateInstance<Sauvegarde>();
    }

    //Evennement lors de l'activation
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargerEvent, Charger);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiUniqueEvent, MortEnnemiUnique);
    }

    //Evennement lors de la désactivation
    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.sauvegardeEvent, Sauvegarder);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargerEvent, Charger);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiUniqueEvent, MortEnnemiUnique);
    }

    /***
     * Gère la conversion et la sauvegarde dans un fichier json
     * @param InfoEvent [evennements reçu]
     * @return void
     */
    void Sauvegarder(InfoEvent evennement) {
        _sauvegarde.positionJoueur = _teddy.transform.position;
        _sauvegarde.pointVieJoueur = _teddyScript.pointDeVie;
        _sauvegarde.pointVieMaxJoueur = _teddyScript.pointDeVieMax;
        _sauvegarde.inventaireJoueur = _teddyScript.inventaireObjet;
        _sauvegarde.inventaireJoueurQte = _teddyScript.inventaireObjetQte;
        _sauvegarde.armeEquipe = _teddyScript.armeActuelle.nom;
        _sauvegarde.inventaireArme = _teddyScript.inventaireArme;
        _sauvegarde.experienceJoueur = _gestionnaireCombat.experience;
        _sauvegarde.experienceMaxJoueur = _gestionnaireCombat.experienceMax;
        _sauvegarde.niveau = _gestionnaireCombat.niveau;
        _sauvegarde.stats = _statScript.RecupererStat();
        _sauvegarde.ventilationEstBriser = _ventilation.estDetruit;
        string stringJson = JsonUtility.ToJson(_sauvegarde);
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
            JsonUtility.FromJsonOverwrite(stringJson, _sauvegarde);
            Debug.Log(_sauvegarde);

            _teddy.transform.position = _sauvegarde.positionJoueur;
            _teddyScript.pointDeVie = _sauvegarde.pointVieJoueur;
            _teddyScript.pointDeVieMax = _sauvegarde.pointVieMaxJoueur;
            _teddyScript.inventaireObjet = _sauvegarde.inventaireJoueur;
            _teddyScript.inventaireObjetQte = _sauvegarde.inventaireJoueurQte;
            _teddyScript.armeActuelle = Resources.Load<ArmeTemplate>("Armes/" + _sauvegarde.armeEquipe); ;
            _teddyScript.inventaireArme = _sauvegarde.inventaireArme;
            _gestionnaireCombat.experience = _sauvegarde.experienceJoueur;
            _gestionnaireCombat.experienceMax = _sauvegarde.experienceMaxJoueur;
            _gestionnaireCombat.niveau = _sauvegarde.niveau;
            _statScript.AssignerStats(_sauvegarde.stats);
            if (_sauvegarde.f1Estmort == true) {
                DetruireUnique("f1Unique");
            }
            if (_sauvegarde.avion1Estmort == true) {
                DetruireUnique("Avion_1");
            }
            if (_sauvegarde.avion2Estmort == true) {
                DetruireUnique("Avion_2");
            }
            if (_sauvegarde.ventilationEstBriser == true) {
                _ventilation.GererDetruireGrille(false);
            }
        }
    }


    private void MortEnnemiUnique(InfoEvent evennement) {
        switch (evennement.Nom) {
            case "f1Unique":
                _sauvegarde.f1Estmort = true;
                DetruireUnique(evennement.Nom);
            break;

            case "Avion_1":
                _sauvegarde.avion1Estmort = true;
                DetruireUnique(evennement.Nom);
            break;

            case "Avion_2":
                _sauvegarde.avion2Estmort = true;
                DetruireUnique(evennement.Nom);
            break;
        }
    }

    private void DetruireUnique(string nom) {
        Destroy(GameObject.Find(nom));
    }
}
