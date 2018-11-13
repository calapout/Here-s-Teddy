using UnityEngine;
using UnityEngine.SystemeEventsLib;
using UnityEngine.UI;

public class ouvrirMenu : MonoBehaviour {

    public GameObject menuPause;
    public GameObject panelNav;
    public GameObject panelArmes;
    Button[] btnsNav;
    InfoEvent infoEvent = new InfoEvent();

	void Start ()
    {
        infoEvent.EnPause = false;
        btnsNav = panelNav.transform.GetComponentsInChildren<Button>();
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelArmes.activeSelf)
            {
                OuvrirPanelArmes();
            }
            else
            {
                OuvrirMenuPause();
            }
        }
    }

    void Resume()
    {
        infoEvent.EnPause = !infoEvent.EnPause;
        SystemeEvents.Instance.LancerEvent(NomEvent.pauseEvent, infoEvent);
    }

    public void OuvrirMenuPause()
    {
        infoEvent.Cible = menuPause;
        Resume();
    }

    public void OuvrirPanelArmes()
    {
        panelArmes.SetActive(!panelArmes.activeSelf);
        if (panelArmes.activeSelf)
        {
            foreach(Button btn in btnsNav)
            {
                btn.interactable = false;
            }
        }
        else
        {
            foreach (Button btn in btnsNav)
            {
                btn.interactable = true;
            }
        }
    }
}
