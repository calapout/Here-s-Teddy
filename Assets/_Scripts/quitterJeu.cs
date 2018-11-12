using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class quitterJeu : MonoBehaviour {

    InfoEvent infoEvent = new InfoEvent();

    public void Quitter()
    {
        SystemeEvents.Instance.LancerEvent(NomEvent.quitterEvent, infoEvent);
    }
}
