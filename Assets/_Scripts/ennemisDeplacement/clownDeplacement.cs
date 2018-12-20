using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe gérant les déplacements, la rotation et des collisions du clown
/// </summary>
/// <remarks>
/// Auteur: Jimmy tremblay-Bernier
/// </remarks>
public class clownDeplacement : MonoBehaviour {
    public GameObject Teddy; //Référence à Teddy
    public float vitesseDeplacement; //contien la vitesse de déplacement du clown

    [HideInInspector]
    public bool _solTouche; //permet de savoir si le clown touche le sol ou non

    private Vector3 _deplacement; //contient les valeur nécéssaire au déplacement
    private Ennemi _ennemi; //contient une référence au script ennemi du clown
    private Transform _enfant; //contient une référence à l'enfant direct contenant le mesh renderer pour pouvoir le faire tourner.

    // Assignation de quelques références
    void Start() {
        Teddy = GameObject.Find("Teddy");
        _ennemi = gameObject.GetComponent<Ennemi>();
        _enfant = transform.GetChild(0);
    }

    // Gère la condition permettant de savoir quelle attaque doit être effectuer et gère le déplacement
    void Update() {
        float distance = (Vector3.Distance(Teddy.transform.position, gameObject.transform.position));

        //Si Teddy est proche du clown, alors ce dernier ce met à tourner
        if (distance < 0.3) {
            _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, _enfant.localEulerAngles.y + 5, _enfant.localEulerAngles.z);
            vitesseDeplacement = 7;
            Deplacement();
        }
        //sinon il va sauter
        else if (distance < 0.7) {
            if (_solTouche == true) {
                gameObject.GetComponent<Rigidbody>().AddForce(0, 0.5f, 0, ForceMode.VelocityChange);
                _solTouche = false;
            }
            vitesseDeplacement = 15;
            Deplacement();

            //détecte la direction de teddy et assige un angle au clown
            if (Teddy.transform.position.x - transform.position.x > 0) {
                _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, 0, _enfant.localEulerAngles.z);
            }
            else {
                _enfant.localEulerAngles = new Vector3(_enfant.localEulerAngles.x, 180, _enfant.localEulerAngles.z);
            }
        }
        //si l'ennemi est trop loins et n'est pas blaisser, alors l'ennemi est détruit sans donner l'exp
        else if (distance > 1.3 && _ennemi.pointsVie == _ennemi.pointsVieMax && !_ennemi.estUnique) {
            _ennemi.Mort(false);
        }

        gameObject.GetComponent<Rigidbody>().velocity = _deplacement;
    }
    /// <summary>
    /// Permet la détection avec le sol
    /// </summary>
    /// <param name="other"></param>
    /// <remarks>le layer 8 équivaut à la couche du postprocessing</remarks>
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != 8) {
            _solTouche = true;
        }
    }
    /// <summary>
    /// Permet la détection avec le sol
    /// </summary>
    /// <param name="other"></param>
    /// <remarks>le layer 8 équivaut à la couche du postprocessing</remarks>
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != 8) {
            _solTouche = true;
        }
    }

    /// <summary>
    /// Fonction Assurant le déplacement du clown en fonction de la position de teddy 
    /// </summary>
    void Deplacement() {
        if ((Teddy.transform.position.x - gameObject.transform.position.x) > 0) {
            _deplacement = new Vector3(vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else {
            _deplacement = new Vector3(-vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
    }
}
