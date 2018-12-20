using UnityEngine;

/**
 * DestructionExplosion.cs
 * Script de néttoyage (destruction) de l'effet d'explosion des bombes
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class DestructionExplosion : MonoBehaviour {

    ParticleSystem ps; //Ref du système de particules

    /**
     * Fonction d'initialisation de la ref du système de particules
     * @param void
     * @return void
     */
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    /**
     * Fonction de vérification de l'arrêt d'émission du système de particules et de sa destruction
     * @param void
     * @return void
     */
    void Update()
    {
        //Si le système de particules est arrêté...
        if (ps.isStopped)
        {
            //Destruction de celui-ci
            Destroy(gameObject);
        }
    }
}