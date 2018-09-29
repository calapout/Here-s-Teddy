using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventCentralization;

public class EventSystemHandler : MonoBehaviour {

    #region SINGLETON
    static EventSystemHandler _instance;
    public static EventSystemHandler Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(EventSystemHandler)) as EventSystemHandler;

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

    Dictionary<EventName, Action<EventInfo>> eventDictionary;

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventName, Action<EventInfo>>();
        }
    }

    public void SubscribeToEvent(EventName eventName, Action<EventInfo> func)
    {
        Action<EventInfo> thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += func;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += func;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void UnsubscribeToEvent(EventName eventName, Action<EventInfo> func)
    {
        Action<EventInfo> thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= func;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(EventName eventName, EventInfo eventInfo)
    {
        Action<EventInfo> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventInfo);
        }
    }
}
