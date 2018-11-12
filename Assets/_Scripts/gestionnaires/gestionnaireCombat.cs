using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;
public class gestionnaireCombat : MonoBehaviour {
    private int _experience;
    private int _experienceMax = 10;
    private int _niveau = 1;
    // Use this for initialization

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    void AjouterExp(InfoEvent infoEvent) {
        Debug.Log("ENNEMIE MORT: " + infoEvent.Experience);
        _experience += infoEvent.Experience;
        Debug.Log("EXPERIENCE ACTUELLE : " + _experience);
        if (_experience - _experienceMax >= 0) {
            NiveauSuperieur();
        }
    }

    void NiveauSuperieur() {
        _experience = _experience - _experienceMax;
        _experienceMax = (int)(_experienceMax * 1.2);
        _niveau++;
        Debug.Log("LEVEL UP: " + _niveau);
        if (_experience > _experienceMax) {
            NiveauSuperieur();
        }
    }
}
