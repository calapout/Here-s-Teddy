using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'animation du poulet, ses attaques et ses déplacements
/// </summary>
/// <remarks>Auteur: Jimmy Tremblay-Bernier</remarks>
public class DeplacementPoulet : MonoBehaviour {

    public float murGauche; //position du mur de gauche
    public float murDroit; //position du mur de droite
    public float vitesseDeplacement; //valeur de vitesse de déplacement
    public AudioClip pouletAttaque; //référence au son d'attaque
    public AudioClip pouletSaut; //référence au son de saut

    [HideInInspector]
    public bool actif = false; //condition d'activation du poulet
    [HideInInspector]
    public bool peutAttaque = true; //condition d'attaque du poulet
    [HideInInspector]
    public bool enSaut = false; //condition de saut du poulet

    private GameObject Teddy; //référence à Teddy
    private AudioSource AS; //référence à l'audioSource
    private Animator _animator; //référence à l'animator

    // Assigne les références aux variables
    void Start () {
        Teddy = GameObject.Find("Teddy");
        _animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        AS = Camera.main.GetComponent<AudioSource>();
    }
	
	// détecte quelle attaque doit être utiliser, sinon il se déplace
	void Update () {
        if (actif == true) {
            
            if (peutAttaque) {
                //si teddy est trop proche du mur alors il saute
                if ((Teddy.transform.position.x - murGauche <= 0.2 || murDroit - Teddy.transform.position.x <= 0.2) && Vector3.Distance(gameObject.transform.position, Teddy.transform.position) < 0.2) {
                    gameObject.GetComponent<Rigidbody>().AddRelativeForce(0.1f, 3.5f, 0, ForceMode.VelocityChange);
                    _animator.SetTrigger("saut");
                    StartCoroutine("DelaiAttaque");
                    enSaut = true;
                    AS.PlayOneShot(pouletSaut);
                }
                //sinon il donne un coup de tête
                else if (Vector3.Distance(gameObject.transform.position, Teddy.transform.position) <= 0.2) {
                    _animator.SetTrigger("attaque");
                    StartCoroutine("DelaiAttaque");
                    AS.PlayOneShot(pouletAttaque, 0.3f);
                }
                else if(enSaut == false) {
                    Deplacement();
                }
            }
            else if(enSaut == false) {
                Deplacement();
            }

        }
	}

    /// <summary>
    /// Lors de la collision avec le sol il réactive sa possibilité de déplacement
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Untagged") {
            enSaut = false;
        }
    }

    /// <summary>
    /// Gère le déplacement et l'orientation du poulet selon Teddy
    /// </summary>
    void Deplacement() {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("deplacement")) {
            if (Teddy.transform.position.x - gameObject.transform.position.x > 0) {
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
                gameObject.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else {
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-vitesseDeplacement * Time.deltaTime, gameObject.GetComponent<Rigidbody>().velocity.y, 0);
                gameObject.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 270, 0);
            }
        }
    }

    /// <summary>
    /// Délai entre chaque attaque
    /// </summary>
    /// <returns></returns>
    IEnumerator DelaiAttaque() {
        peutAttaque = false;
        yield return new WaitForSeconds(2f);
        peutAttaque = true;
    }
}
