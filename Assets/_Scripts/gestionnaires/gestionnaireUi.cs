using UnityEngine;
using UnityEngine.SystemeEventsLib;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class gestionnaireUi : MonoBehaviour {

    public GameObject panelPrincipal;
    public GameObject panelEtat;
    public GameObject panelExp;
    public GameObject niveau;
    GameObject barreVie;
    GameObject barreExp;

    void OnEnable()
    {
        SystemeEvents.Instance.AbonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.AbonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
    }

    void OnDisable()
    {
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.pauseEvent, PauseEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.chargementEvent, ChargementEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.quitterEvent, QuitterEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiVieEvent, UpdateUiVieEvent);
        SystemeEvents.Instance.DesabonnementEvent(NomEvent.updateUiExpEvent, UpdateUiExpEvent);
    }

    void Start()
    {
        barreVie = panelPrincipal.transform.GetChild(0).gameObject;
        barreExp = panelPrincipal.transform.GetChild(2).gameObject;
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

    void UpdateUiVieEvent(InfoEvent info)
    {
        UpdateVie(info.HP, info.HPMax);
    }

    void UpdateUiExpEvent(InfoEvent info)
    {
        UpdateExp(info.Experience, info.ExpMax, info.ExpTotal, info.ExpNextNiveau, info.Niveau);
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

    void UpdateVie(int vie, int vieMax)
    {
        barreVie.GetComponent<Slider>().value = vie;
        barreVie.GetComponent<Slider>().maxValue = vieMax;
        panelEtat.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = vie.ToString() + "/" + vieMax;
    }

    void UpdateExp(int exp, int expMax, int expTotal, int expNextNiveau, int niv)
    {
        barreExp.GetComponent<Slider>().value = exp;
        barreExp.GetComponent<Slider>().maxValue = expMax;
        panelExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = expTotal.ToString();
        panelExp.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = expNextNiveau.ToString();
        niveau.GetComponent<TextMeshProUGUI>().text = niv.ToString();
    }
}