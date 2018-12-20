using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le déplacement, la rotation, l'attaque et l'animation de l'avion
/// </summary>
/// <remarks>auteur: Jimmy tremblay-bernier</remarks>

public class DeplacementAvion : MonoBehaviour {
    public GameObject Teddy; // référence au gameObject du joueur
    public float vitesseDeplacement; //stockage de la vitesse de déplacement
    public GameObject arme; //référence au prefab de la bombe à larguer

    [HideInInspector]
    public bool estEnAttaque = false; //condition pour savoir si kl'avion peut attaquer
    [HideInInspector]
    public bool tourne = false; //condition pour faire tourner l'avion

    private Vector3 _deplacement; // Contient les donées nécessaire au déplacement
    private Ennemi _ennemi; //contient une référence au script ennemi
    private Transform _enfant; //contient le gameObject ayant le renderer. Utiliser pour la rotation.

    //Assignation de quelques références.
    void Start() {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        _enfant = transform.GetChild(0);
        //si l'avion est NAGAZAKI, alors on le désactive de base
        if (gameObject.name == "NAGAZAKI") {
            gameObject.SetActive(false);
        }
    }

    // gère les conditions d'attaque et de mouvement de l'avion
    void Update() {
        float distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));
        //Si l'avion est au dessus de teddy alors on largue une bombe
        if (estEnAttaque == false && (Teddy.transform.position.x > transform.position.x - 0.3 || Teddy.transform.position.x > transform.position.x + 0.3)) {
            Attaquer();
        }
        //on permet de tourner
        if (distance < 1.2) {
            tourne = false;
        }
        //on tourne
        else if (distance > 1.2) {
            if (tourne == false) {
                Tourner();
            }
        }

        Deplacement();
    }

    /// <summary>
    /// Si l'avion détecte un mur alors il tourne
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player" || other.gameObject.layer != 8) {
            Tourner();
        }
    }

    /// <summary>
    /// Si l'avion détecte un mur alors il tourne
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag != "Player" || other.gameObject.layer != 8) {
            Tourner();
        }
    }

    /// <summary>
    /// Gère le déplacement de l'avion selon son angle
    /// </summary>
    void Deplacement() {
        if (_enfant.eulerAngles.y / 90 == 1) {
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(vitesseDeplacement * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
        else {
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(-vitesseDeplacement * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Gère l'angle ou l'avion doit tourner selon son angle interne et selon teddy
    /// </summary>
    void Tourner() {

        int rotation;
        //vérifier l'angle à atteindre selon la position de Teddy
        if (Teddy.transform.position.x - gameObject.transform.position.x > 0) {
            rotation = 90;
        }
        else {
            rotation = 270;
        }

        //si l'angle est différent de celui actuel, alors on l'assigne et on annule la velocité actuelle
        if (_enfant.eulerAngles.y != rotation) {
            tourne = true;
            _enfant.eulerAngles = new Vector3(0, rotation, 0);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Fait apparaitre une bombe et désactive la possibilité d'attaquer tant que la bombe existe
    /// </summary>
    void Attaquer() {
        estEnAttaque = true;
        GameObject bombe = Instantiate(arme);
        bombe.GetComponent<Bombe>().degats = _ennemi.degats;
        bombe.GetComponent<Bombe>().parent = gameObject;
        bombe.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f, gameObject.transform.position.z);
    }
}
