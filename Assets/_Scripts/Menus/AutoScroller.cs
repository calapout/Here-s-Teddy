using System.Collections;
using UnityEngine;
using TMPro;

/**
 * AutoScroller.cs
 * Script de gestion du défillement du générique et de l'affichage des remerciments dans la scène de fin
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class AutoScroller : MonoBehaviour {

    public RectTransform contenu; //RectTransform du contenu du générique (parent du texte)
    public TextMeshProUGUI remerciement; //Texte de remerciment
    public float vitesse; //Vitesse du défillement
    [Header("    Positions")]
    public Vector3 positionInitial; //Position initial du générique
    public Vector3 positionArret; //Position finale du générique

    IEnumerator CoroutineDefillement; //Coroutine du défillement
    float vitesseTransition = 0.005f; //Vitesse d'apparition du texte de remerciment

    /**
     * Fonction d'initialisation de la coroutine
     * @param void
     * @return void
     */
    void Start () {
        CoroutineDefillement = Defillement();
        StartCoroutine(CoroutineDefillement);
    }

    /**
     * Coroutine de défillement et d'estompage
     * @param void
     * @return void
     */
    IEnumerator Defillement()
    {
        contenu.localPosition = new Vector3(contenu.localPosition.x, positionInitial.y, 0);
        //Fait défiller le contenu du générique
        while (contenu.localPosition.y <= positionArret.y)
        {
            contenu.localPosition = new Vector3(contenu.localPosition.x, contenu.localPosition.y + vitesse, 0);
            yield return null;
        }

        //Transitionne l'alpha des remerciment pour les faire apparaître
        while (remerciement.color.a < 1)
        {
            remerciement.color = new Color(remerciement.color.r, remerciement.color.g, remerciement.color.b, remerciement.color.a + vitesseTransition);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(5);

        //Transitionne l'alpha des remerciment pour les faire disparaître
        while (remerciement.color.a > 0)
        {
            remerciement.color = new Color(remerciement.color.r, remerciement.color.g, remerciement.color.b, remerciement.color.a - vitesseTransition);
            yield return null;
        }
        StopAllCoroutines();
    }
}