using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class activerPause : MonoBehaviour {

    public GameObject menuPause;
    public GameObject menuPerso;
    InfoEvent infoEvent = new InfoEvent();

	void Start () {
        infoEvent.EnPause = false;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
	}

    public void Resume()
    {
        infoEvent.Cible = menuPause;
        infoEvent.EnPause = infoEvent.EnPause ? false : true;
        SystemeEvents.Instance.LancerEvent(NomEvent.pauseEvent, infoEvent);
    }
}
