using System.Collections;
using UnityEngine;

/**
 * Bombe.cs
 * Script de gestion du comportement des bombes larguées par les avions
 * @author Jimmy Tremblay-Bernier
 * @version Mercredi 19 Décembre 2018
 */
public class Bombe : MonoBehaviour {
    public GameObject parent; //Ref du parent (avion)
    public int degats; //Dégâts infligés par la bombe

    /**
     * Fonction de vérification d'entrée en collision avec n'importe quel collider
     * @param Collision collision
     * @return void
     */
    private void OnCollisionEnter(Collision collision) {
        GetComponent<InstanciationExplosion>().Boom(); //Active l'explosion de la bombe (ParticuleSystem)
        StartCoroutine("Delai"); //Délais avant destruction
        //Désactivation visuelle de la bombe lors de l'entrée en contact
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    /**
     * Coroutine qui ajoute un delais avant la destruction de la bombe
     * @param void
     * @return void
     */
    IEnumerator Delai() {
        yield return new WaitForSeconds(1f);
        Detruire();
    }

    /**
     * Fonction de destruction de la bombe
     * @param void
     * @return void
     */
    void Detruire() {
        StopAllCoroutines();
        //Si la ref du parent n'est pas null...
        if (parent != null) {
            //L'avion n'est pas en attaque
            parent.GetComponent<DeplacementAvion>().estEnAttaque = false;
        }
        Destroy(gameObject);
    }
}