using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe qui gère la sauvegarde et le chargement dans un fichier json
 * @author Jimmy Tremblay-Bernier
 */
public class GestionnaireSauvegarde : MonoBehaviour {
    private GameObject _teddy; //référence à teddy
    private joueur _teddyScript; //référence au script joueur
    private Statistiques _statScript; //référence au script statistique
    private gestionnaireCombat _gestionnaireCombat; //référence au gestionnaire de combat
    private conduitScript _ventilation; //référence à la ventilation
    private Sauvegarde _sauvegarde; //référence au scriptableObejct permettant la sauvegarde

    // assigne les référence aux variables et créer un instance de Sauvegarde
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
        _sauvegarde.armeEquipe = _teddyScript.armeActuelle.nom;
        _sauvegarde.inventaireArme = _teddyScript.inventaireArme;

        _sauvegarde.experienceJoueur = _gestionnaireCombat.experience;
        _sauvegarde.experienceMaxJoueur = _gestionnaireCombat.experienceMax;
        _sauvegarde.experienceJoueurTotal = _gestionnaireCombat.experienceTotal;
        _sauvegarde.experienceJoueurNextLevel = _gestionnaireCombat.experienceNextLevel;

        _sauvegarde.niveau = _gestionnaireCombat.niveau;

        _sauvegarde.rage = _teddyScript.GetComponent<Rage>().pointsDeRage;

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
        if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {

            string stringJson = System.IO.File.ReadAllText("Assets/Resources/Saves/save.json");
            JsonUtility.FromJsonOverwrite(stringJson, _sauvegarde);
            _teddy.transform.position = _sauvegarde.positionJoueur;

            InfoEvent info = new InfoEvent();
            //HP
            info.HP = _sauvegarde.pointVieJoueur;
            _teddyScript.pointDeVie = _sauvegarde.pointVieJoueur;

            info.HPMax = _sauvegarde.pointVieMaxJoueur;
            _teddyScript.pointDeVieMax = _sauvegarde.pointVieMaxJoueur;

            //exp
            _gestionnaireCombat.experience = _sauvegarde.experienceJoueur;
            info.Experience = _sauvegarde.experienceJoueur;

            _gestionnaireCombat.experienceMax = _sauvegarde.experienceMaxJoueur;
            info.ExpMax = _sauvegarde.experienceMaxJoueur;

            _gestionnaireCombat.experienceTotal = _sauvegarde.experienceJoueurTotal;
            info.ExpTotal = _sauvegarde.experienceJoueurTotal;

            _gestionnaireCombat.experienceNextLevel = _sauvegarde.experienceJoueurNextLevel;
            info.ExpNextNiveau = _sauvegarde.experienceJoueurNextLevel;

            //niveau
            _gestionnaireCombat.niveau = _sauvegarde.niveau;
            info.Niveau = _sauvegarde.niveau;

            _teddyScript.armeActuelle = Resources.Load<ArmeTemplate>("Armes/" + _sauvegarde.armeEquipe);
            _teddyScript.inventaireArme = _sauvegarde.inventaireArme;
            _teddyScript.inventaireArmeTemplates.RemoveAt(0);
            for (int i = 0; i < _teddyScript.inventaireArme.Count; i++) {

                _teddyScript.inventaireArmeTemplates.Insert(i, Resources.Load<ArmeTemplate>("Armes/" + _sauvegarde.inventaireArme[i]));
            }

            //rage
            _teddyScript.GetComponent<Rage>().pointsDeRage = _sauvegarde.rage;
            info.Rage = _sauvegarde.rage;
            info.RageMax = 50;

            //lancement des évênements d'UI
            SystemeEvents.Instance.LancerEvent(NomEvent.initEvent, info);
            SystemeEvents.Instance.LancerEvent(NomEvent.updateUiExpEvent, info);

            //meurtre des ennemis uniques et de la ventilation
            if (_sauvegarde.f1Estmort == true) {
                DetruireUnique("f1Unique");
            }
            if (_sauvegarde.avion1Estmort == true) {
                DetruireUnique("HIROSHIMA");
            }
            if (_sauvegarde.avion2Estmort == true) {
                DetruireUnique("activationNAGAZAKI");
            }
            if (_sauvegarde.ventilationEstBriser == true) {
                _ventilation.GererDetruireGrille(false);
            }
        }
    }

    /// <summary>
    /// Tue l'ennemi unique par le nom de son GameObject
    /// </summary>
    /// <param name="evennement">Nom du GameObject à détruire</param>
    private void MortEnnemiUnique(InfoEvent evennement) {
        switch (evennement.Nom) {
            case "f1Unique":
            _sauvegarde.f1Estmort = true;
            DetruireUnique(evennement.Nom);
            break;

            case "HIROSHIMA":
            _sauvegarde.avion1Estmort = true;
            DetruireUnique(evennement.Nom);
            break;

            case "NAGAZAKI":
            _sauvegarde.avion2Estmort = true;
            DetruireUnique(evennement.Nom);
            break;
        }
    }

    //Destruction du gameObject
    private void DetruireUnique(string nom) {
        Destroy(GameObject.Find(nom));
    }
}
