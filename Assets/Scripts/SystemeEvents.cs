using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class SystemEvents : MonoBehaviour {

    #region SINGLETON
    static SystemEvents _instance;
    public static SystemEvents Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SystemEvents)) as SystemEvents;

                if (_instance)
                {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    #endregion

    Dictionary<NomEvent, Action<InfoEvent>> dictionaireEvents;

    void Init()
    {
        if (dictionaireEvents == null)
        {
            dictionaireEvents = new Dictionary<NomEvent, Action<InfoEvent>>();
        }
    }

    public void AbonnementEvent(NomEvent nomEvent, Action<InfoEvent> fonc)
    {
        Action<InfoEvent> cetEvent;
        if (Instance.dictionaireEvents.TryGetValue(eventName, out cetEvent))
        {
            cetEvent += fonc;
            Instance.dictionaireEvents[nomEvent] = cetEvent;
        }
        else
        {
            cetEvent += fonc;
            Instance.dictionaireEvents.Add(nomEvent, cetEvent);
        }
    }

    public void DesabonnementEvent(NomEvent nomEvent, Action<InfoEvent> fonc)
    {
        Action<InfoEvent> cetEvent;
        if (Instance.dictionaireEvents.TryGetValue(nomEvent, out cetEvent))
        {
            cetEvent -= fonc;
            Instance.dictionaireEvents[nomEvent] = cetEvent;
        }
    }

    public void LancerEvent(NomEvent nomEvent, InfoEvent infoEvent)
    {
        Action<InfoEvent> cetEvent = null;
        if (Instance.dictionaireEvents.TryGetValue(nomEvent, out cetEvent))
        {
            cetEvent.Invoke(infoEvent);
        }
    }
}
