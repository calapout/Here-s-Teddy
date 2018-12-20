using UnityEngine;

/**
 * InstanciationExplosion.cs
 * Gére l'instanciation des bombes larguer par les avions
 * @author Catherine Beaudoin-Rheault 
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class InstanciationExplosion : MonoBehaviour {

    AudioSource audioSource; //Variable de l'audiosource
    Vector3 position; //Postion de l'explosion

    /**
     * Fonction qui permet d'activer l'explosion (ParticleSystem) et de jouer son son
     * @param void
     * @return void
     */
    public void Boom()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
        position = transform.position;
        //Instanciation de l'explosion depuis le répertoire des ressources
        Instantiate(Resources.Load("Explosion_Res") as GameObject, position, new Quaternion());
    }
}