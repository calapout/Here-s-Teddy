using System.Collections;
using UnityEngine;
using TMPro;

public class AutoScroller : MonoBehaviour {

    public RectTransform contenu;
    public TextMeshProUGUI remerciement;
    public float vitesse;
    [Header("    Positions")]
    public Vector3 positionInitial;
    public Vector3 positionArret;

    IEnumerator CoroutineDefillement;
    float vitesseTransition = 0.005f;

	void Start () {
        CoroutineDefillement = Defillement();
        StartCoroutine(CoroutineDefillement);
    }

    IEnumerator Defillement()
    {
        contenu.localPosition = new Vector3(contenu.localPosition.x, positionInitial.y, 0);
        while (contenu.localPosition.y <= positionArret.y)
        {
            contenu.localPosition = new Vector3(contenu.localPosition.x, contenu.localPosition.y + vitesse, 0);
            yield return null;
        }

        while (remerciement.color.a < 1)
        {
            remerciement.color = new Color(remerciement.color.r, remerciement.color.g, remerciement.color.b, remerciement.color.a + vitesseTransition);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(5);

        while (remerciement.color.a > 0)
        {
            remerciement.color = new Color(remerciement.color.r, remerciement.color.g, remerciement.color.b, remerciement.color.a - vitesseTransition);
            yield return null;
        }
        StopAllCoroutines();
    }
}