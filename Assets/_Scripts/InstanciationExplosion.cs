
/**
 * Gére l'instanciation de la bombe
 * @author Catherine Beaudoin-Rheault et Yoann Paquette
 * @version 2018-12-20
 * */
using UnityEngine;

public class InstanciationExplosion : MonoBehaviour {

    AudioSource audioSource;//variable de l'audiosource
    Vector3 position;//postion de l'explosion
    

    /*Fonction qui permet d'activé l'explosion et de jouer son son*/
    public void Boom()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
        position = transform.position;
        Instantiate(Resources.Load("Explosion_Res") as GameObject, position, new Quaternion());
    }
}