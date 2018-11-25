using UnityEngine;
using UnityEngine.EventCentralization;

public class EnnemiScript : MonoBehaviour {

    public int id;
    public int health;
    bool isDead = false;
    EventInfo eventInfo = new EventInfo();

    void Start() {
        eventInfo.Id = id;
        eventInfo.Target = gameObject;
        eventInfo.Damage = 2;
    }

    void Update() {
        if (health <= 0 && !isDead) {
            isDead = true;
            EventSystemHandler.Instance.TriggerEvent(EventName.OnDeath, eventInfo);
        }
    }
}