using UnityEngine;
using UnityEngine.EventCentralization;

public class PlayerScript : MonoBehaviour {

    public int damage = 2;

    RaycastHit hit;

    EventInfo eventInfo = new EventInfo();

    void Start()
    {
        eventInfo.Damage = damage;
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("LEFT MOUSE BUTTON PRESSED");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ennemi")
                {
                    Debug.Log("RAYCAST HIT ENNEMI");
                    eventInfo.Target = hit.collider.gameObject;
                    eventInfo.Id = eventInfo.Target.GetComponent<EnnemiScript>().id;
                    EventSystemHandler.Instance.TriggerEvent(EventName.OnAttack, eventInfo);
                }
                else
                {
                    Debug.Log("RAYCAST HIT SOMETHING");
                }
            }
        }
	}
}
