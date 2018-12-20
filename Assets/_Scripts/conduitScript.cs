using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère la destruction de la ventilation lors du contacte avec une arme
/// </summary>
/// <remarks>auteur: Jimmy Tremblay-Bernier</remarks>
public class conduitScript : MonoBehaviour {
    [HideInInspector]
    public bool estDetruit = false; //permet de svoir si la grille est brisée ou non

    private Transform[] _enfant = new Transform[7]; //contient la référence au moreceaux de métal enfants

    //assigne les enfants à _enfant
    private void Start() {
        for (int i = 0; i < 7; i++) {
            _enfant[i] = gameObject.transform.GetChild(i);
        }
    }

    ///<summary>
    /// Enclence la destruction lors du collision avec une une arme
    ///</summary>
    ///<param name="collision"></param>
    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "arme" && estDetruit == false) {
            GererDetruireGrille(true);
        }
    }

    /// <summary>
    /// Gere la destruction de la ventilation
    /// </summary>
    /// <param name="animation">Permet de svoir s'il faut jouer l'animation ou non</param>
    public void GererDetruireGrille(bool animation) {
        estDetruit = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        if (animation == true) {
            for (int i = 0; i < 7; i++) {
                _enfant[i].SetParent(null);
                _enfant[i].gameObject.GetComponent<Rigidbody>().AddForce(0.5f, 0, 0, ForceMode.Impulse);
                _enfant[i].gameObject.GetComponent<Rigidbody>().useGravity = true;
                _enfant[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            StartCoroutine("DetruireGrille", 3);
        }
        else {
            DetruireEnfant();
        }
    }


    private float _pourcentage;
    /// <summary>
    /// retourne la valeur à déduire de l'opacité des pièces durant l'animation selon un temps donner
    /// </summary>
    /// <param name="seconde">La durée de l'animation</param>
    /// <returns>la valeur à déduire de l'opacité</returns>
    IEnumerator DetruireGrille(float seconde) {
        _pourcentage = 1 / seconde;
        InvokeRepeating("BaisserOpacite", 0, 1);
        yield return new WaitForSeconds(seconde);
        CancelInvoke();

        DetruireEnfant();
        StopAllCoroutines();
    }


    /// <summary>
    /// Détruit les enfant
    /// </summary>
    void DetruireEnfant() {
        for (int i = 0; i < 7; i++) {
            Destroy(_enfant[i].gameObject);
        }
    }

    /// <summary>
    /// Baisse l'opacité selon la valeur de _pourcentage
    /// </summary>
    void BaisserOpacite() {
        for (int i = 0; i < 7; i++) {
            var couleur =_enfant[i].GetComponent<MeshRenderer>().material.color;
            couleur.a -= _pourcentage;
            _enfant[i].GetComponent<MeshRenderer>().material.color = couleur;
        }
    }
}
