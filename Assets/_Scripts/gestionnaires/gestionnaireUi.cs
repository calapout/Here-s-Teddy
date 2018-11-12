using UnityEngine;
using UnityEngine.SystemeEventsLib;
using UnityEngine.SceneManagement;

public class gestionnaireUi : MonoBehaviour {

    void OnEnable()
    {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.quitterEvent, QuitterEvent);
    }

    void OnDisable()
    {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.quitterEvent, QuitterEvent);
    }

    void PauseEvent(InfoEvent info)
    {
        if (info.EnPause)
        {
            Pause(info.Cible);
        }
        else
        {
            Resume(info.Cible);
        }
    }

    void ChargementEvent(InfoEvent info)
    {
        ChargerMenu();
    }

    void QuitterEvent(InfoEvent info)
    {
        QuitterJeu();
    }

    void Pause(GameObject ui)
    {
        ui.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume(GameObject ui)
    {
        ui.SetActive(false);
        Time.timeScale = 1f;
    }

    void ChargerMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void QuitterJeu()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }

}
