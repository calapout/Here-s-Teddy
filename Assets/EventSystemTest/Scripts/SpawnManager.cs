using UnityEngine;
using UnityEngine.EventCentralization;

public class SpawnManager : MonoBehaviour {

    #region SINGLETON
    static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SpawnManager)) as SpawnManager;
            }
            return _instance;
        }
    }
    #endregion

    void OnEnable()
    {
        EventSystemHandler.Instance.SubscribeToEvent(EventName.OnDeath, OnDeathEvent);
    }

    void OnDisable()
    {
        EventSystemHandler.Instance.UnsubscribeToEvent(EventName.OnDeath, OnDeathEvent);
    }

    void OnDeathEvent(EventInfo info)
    {
        Debug.Log("<color=red>" + info.Target.name + "</color> has died. [ID=<color=blue>" + info.Id + "</color>]");
        Kill(info.Target);
    }

    void Kill(GameObject target)
    {
        Destroy(target);
    }
}
