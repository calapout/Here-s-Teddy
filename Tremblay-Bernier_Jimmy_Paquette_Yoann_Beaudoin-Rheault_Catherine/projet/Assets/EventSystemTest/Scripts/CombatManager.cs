using UnityEngine;
using UnityEngine.EventCentralization;

public class CombatManager : MonoBehaviour {

    #region SINGLETON
    static CombatManager _instance;
    public static CombatManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CombatManager)) as CombatManager;
            }
            return _instance;
        }
    }
    #endregion

    void OnEnable()
    {
        EventSystemHandler.Instance.SubscribeToEvent(EventName.OnAttack, OnAttackEvent);
    }

    void OnDisable()
    {
        EventSystemHandler.Instance.UnsubscribeToEvent(EventName.OnAttack, OnAttackEvent);
    }

    void OnAttackEvent(EventInfo info)
    {
        Debug.Log("<color=green>Player</color> is attacking <color=red>" + info.Target + "</color>");
        TakingDamage(info);
    }

    public void TakingDamage(EventInfo info)
    {
        info.Target.GetComponent<EnnemiScript>().health -= info.Damage;
        info.HealthUI = info.Target.transform.GetChild(0).GetChild(0).gameObject;
        info.Health = info.Target.GetComponent<EnnemiScript>().health;
        EventSystemHandler.Instance.TriggerEvent(EventName.OnDamage, info);
    }
}
