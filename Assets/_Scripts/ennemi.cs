using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class ennemi : MonoBehaviour {

    public int pointsVie;
    public GameObject Teddy;
    public int experience;
    public GameObject spawner;

    private InfoEvent evennement = new InfoEvent();

    private void Start()
    {
        Teddy = GameObject.Find("Teddy");
        evennement.Experience = experience;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "arme") {
            Debug.Log("ici");
            pointsVie -= Teddy.GetComponent<joueur>().arme.gameObject.GetComponent<arme>().degats;
            if (pointsVie <= 0) {
                Mort();
            }
        }
    }

    void Mort() {
        SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, evennement);
        spawner.GetComponent<spawner>().estMort = true;
        Destroy(gameObject);
    }
}
