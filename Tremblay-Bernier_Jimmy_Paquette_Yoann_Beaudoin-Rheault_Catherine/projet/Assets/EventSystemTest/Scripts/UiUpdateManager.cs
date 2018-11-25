using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventCentralization;

public class UiUpdateManager : MonoBehaviour {

    #region SINGLETON
    static UiUpdateManager _instance;
    public static UiUpdateManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UiUpdateManager)) as UiUpdateManager;
            }
            return _instance;
        }
    }
    #endregion

    public Canvas canvas;

    void OnEnable()
    {
        EventSystemHandler.Instance.SubscribeToEvent(EventName.OnDamage, OnDamageEvent);
    }

    void OnDisable()
    {
        EventSystemHandler.Instance.UnsubscribeToEvent(EventName.OnDamage, OnDamageEvent);
    }

    void OnDamageEvent(EventInfo info)
    {
        Debug.Log("<color=red>" + info.Target.name + "</color> has taken damage. ( <color=yellow>" + info.Health + "</color> HP remaining) [ID=<color=blue>" + info.Id + "</color>]");
        UpdateHealth(info.Health, info.HealthUI);
    }

    void UpdateScore(int score)
    {
        canvas.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
    }

    void UpdateHealth(int health, GameObject ui)
    {
        ui.GetComponent<Text>().text = health.ToString();
    }
}
