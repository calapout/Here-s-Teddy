using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class gestionnaireCombat : MonoBehaviour {
    public int experience;
    public int experienceMax = 10;
    public int niveau = 1;
    InfoEvent infoEvent2 = new InfoEvent();

    private void OnEnable() {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    private void OnDisable() {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.mortEnnemiEvent, AjouterExp);
    }

    void Start()
    {
        infoEvent2.ExpMax = experienceMax;
        infoEvent2.ExpNextNiveau = experienceMax;
    }

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
