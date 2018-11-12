using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class gestionnaireCombat : MonoBehaviour {
    public int experience;
    public int experienceMax = 10;
    public int niveau = 1;
    // Use this for initialization

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    void AjouterExp(InfoEvent infoEvent) {
        Debug.Log("ENNEMIE MORT: " + infoEvent.Experience);
        experience += infoEvent.Experience;
        Debug.Log("EXPERIENCE ACTUELLE : " + experience);
        if (experience - experienceMax >= 0) {
            NiveauSuperieur();
        }
    }

    void NiveauSuperieur() {
        experience = experience - experienceMax;
        experienceMax = (int)(experienceMax * 1.2);
        niveau++;
        Debug.Log("LEVEL UP: " + niveau);
        if (experience > experienceMax) {
            NiveauSuperieur();
        }
    }
}
