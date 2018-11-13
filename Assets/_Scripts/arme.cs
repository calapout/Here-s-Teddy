using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Classe gérant l'arme à laquelle le script est affecter
 * @author Jimmy Tremblay-Bernier
 */

public class arme : MonoBehaviour {

    //variable publique
    public float tempDesactivation;
    public int degats;

    //Evennement lors de l'activation
    private void OnEnable() {
        StartCoroutine("Desactiver", 0.5f);
    }

    /***
     * désactive l'arme après un nombre de seconde donné en paramètre
     * @param float [nombre de seconde avant la désactivation]
     * @return yield [WaitForSeconds]
     */
    IEnumerator Desactiver(float secondes) {
        yield return new WaitForSeconds(secondes);
        Detruire();
    }

    /***
     * désactive l'arme
     * @param void
     * @return void
     */
    void Detruire() {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
