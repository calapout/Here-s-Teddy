using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class chargerMenu : MonoBehaviour {

    InfoEvent infoEvent = new InfoEvent();

    public void Charger()
    {
        SystemeEvents.Instance.LancerEvent(NomEvent.chargementEvent, infoEvent);
    }
}
