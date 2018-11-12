using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SystemeEventsLib;

public class ennemi : MonoBehaviour {
    public bool kill;
    public int pointsVie;
    public int pointsVieMax;
    public int degats;
    public GameObject Teddy;
    public int experience;
    public GameObject spawner;
    public GameObject recompense;
    public float decalageYRecompense;
    public float chanceLoot;

    private InfoEvent _evennement = new InfoEvent();

    private void Update() {
        if (kill == true || (Vector3.Distance(Teddy.transform.position, gameObject.transform.position) > 1.1 && pointsVie == pointsVieMax)) {
            Mort(false);
        }
    }

    private void Start() {
        Teddy = GameObject.Find("Teddy");
        _evennement.Experience = experience;
        pointsVie = pointsVieMax;
    }

    private void OnTriggerEnter(Collider collision) {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "arme") {
            Debug.Log("ici");
            pointsVie -= Teddy.GetComponent<joueur>().arme.gameObject.GetComponent<arme>().degats;
            if (pointsVie <= 0) {
                Mort();
            }
        }
    }

    void Mort(bool recompenseTrigger = true) {
        if (recompenseTrigger == true) {
            SystemeEvents.Instance.LancerEvent(NomEvent.mortEnnemiEvent, _evennement);
        }
        if (DoitRecompenser(chanceLoot) && recompenseTrigger == true) {
            var recompenseTemp = Instantiate(recompense);
            recompenseTemp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + decalageYRecompense, gameObject.transform.position.z);
            recompenseTemp.name = recompense.name;
        }
        spawner.GetComponent<spawner>().estMort = true;
        Destroy(gameObject);
    }

    bool DoitRecompenser(float pourcentage) {
        int aleatoire = Random.Range(0, 101);
        return aleatoire <= pourcentage;
    }
}
