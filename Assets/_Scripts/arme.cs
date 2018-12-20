using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Classe gérant l'arme à laquelle le script est affecter
 * @author Jimmy Tremblay-Bernier
 * @author Yoann Paquette
 */

public class arme : MonoBehaviour {

    //variable publique
    public float tempDesactivation;
    public int degats;
    int degatsTemp = 0;
    public int degatsTotal = 0;
    float attaqueTemp = 0f;
    Statistiques statsRef;

    //Evennement lors de l'activation
    private void OnEnable() {
        StartCoroutine("Desactiver", 0.5f);
    }

    void Awake()
    {
        statsRef = transform.parent.GetComponent<Statistiques>();
    }

    void Update()
    {
        if (degatsTemp != degats || attaqueTemp != statsRef.Attaque.Stat)
        {
            degatsTemp = degats;
            attaqueTemp = statsRef.Attaque.Stat;
            degatsTotal = degats + (int)statsRef.Attaque.Stat;
        }
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
