using UnityEngine;
using UnityEngine.SystemeEventsLib;

/***
 * Classe controlant les mouvements du personnage, ses points de vies et son inventaire.
 * @author Jimmy Tremblay-Bernier
 * @author Yoann paquette
 */

public class gestionnaireCombat : MonoBehaviour {
    // variables publiques
    public int experience;
    public int experienceMax = 10;
    public int niveau = 1;

    // variables privée
    InfoEvent infoEvent2 = new InfoEvent();

    // évênnement d'activation
    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    // évênnement de désactivation
    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    // évênnement de départ
    void Start() {
        infoEvent2.ExpMax = experienceMax;
        infoEvent2.ExpNextNiveau = experienceMax;
    }

    /***
     * gère l'ajout des points d'expériences, lances des évênnements pour l'affichage et permet la monter de niveau
     * @param InfoEvent [paramètre contenant toutes les informations nécéssaire comme le nombre de point d'expérience à ajouter]
     * @return void
     */
    void AjouterExp(InfoEvent infoEvent) {
        Debug.Log("ENNEMIE MORT: " + infoEvent.Experience);
        experience += infoEvent.Experience;
        infoEvent2.ExpTotal += infoEvent.Experience;
        Debug.Log("EXPERIENCE ACTUELLE : " + experience);
        if (experience - experienceMax >= 0) {
            NiveauSuperieur();
        }
        infoEvent2.Experience = experience;
        infoEvent2.ExpMax = experienceMax;
        infoEvent2.Niveau = niveau;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiExpEvent, infoEvent2);
    }
    /***
     * gère la monté de niveaux
     * @param void
     * @return void
     */
    void NiveauSuperieur() {
        experience = experience - experienceMax;
        experienceMax = (int)(experienceMax * 1.2);
        infoEvent2.ExpNextNiveau += experienceMax;
        niveau++;
        Debug.Log("LEVEL UP: " + niveau);
        if (experience > experienceMax) {
            NiveauSuperieur();
        }
    }
}
