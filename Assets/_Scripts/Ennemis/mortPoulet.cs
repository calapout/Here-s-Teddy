using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Gère l'apparition de l'écran de victoire
/// </summary>
/// <remarks>Auteur: Jimmy tremblay-Bernier</remarks>
public class mortPoulet : StateMachineBehaviour {

    public AudioSource AS;
    public AudioClip pouletMort;

    //se joue lorsque l'animation de mort du poulet est finie
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(pouletMort);
        GameObject.Find("ConditionDeFin").GetComponent<ConditionDeFin>().aGagner = true;
        SceneManager.LoadScene("fin");
	}
}
