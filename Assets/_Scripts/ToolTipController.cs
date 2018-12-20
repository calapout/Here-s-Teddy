using System.Collections;
using UnityEngine;
using TMPro;

/**
 * ToolTipController.cs
 * Script de gestion de l'affichage des infobulles d'information dans le menu pause
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class ToolTipController : MonoBehaviour {

    public GameObject tooltipAnchor; //Ref du parent des infobulles
    TextMeshProUGUI tooltipTexte; //Ref du texte des infobulles
    IEnumerator CoroutineAffichage; //Coroutine qui délais l'affichage

    RectTransform tooltipParentRectTransform; //Ref du RectTransform du Canvas
    RectTransform tooltipAnchorRectTransform; //Ref du RectTransform du parent

    public Vector3 tooltipOffset; //Offset de la position des infobulles

    /**
     * Fonction d'initialisation des références des variables
     * @param void
     * @return void
     */
    void Start () {
        tooltipTexte = tooltipAnchor.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tooltipParentRectTransform = tooltipAnchor.transform.parent.GetComponent<RectTransform>();
        tooltipAnchorRectTransform = tooltipAnchor.GetComponent<RectTransform>();
    }

    /**
     * Fonction de modification de la position des infobulles selon la positionde la souris et de la résolution d'écran
     * @param void
     * @return void
     */
    void Update()
    {
        //Si l'infobulle est activé...
        if (tooltipAnchor.activeSelf)
        {
            //Modifier la position
            tooltipAnchor.transform.position = Input.mousePosition + tooltipOffset;
            ClampToolTip(); //Limitation de la position à l'écran
        }
    }

    /**
     * Fonction d'affichage de l'infobulle
     * @param string texte (Texte de l'infobulle)
     * @return void
     */
    public void AfficherToolTip(string texte)
    {
        tooltipTexte.text = texte;
        CoroutineAffichage = Afficher();
        StartCoroutine(CoroutineAffichage);
    }

    /**
     * Coroutine du délais de l'affichage
     * @param void
     * @return void
     */
    IEnumerator Afficher()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        tooltipAnchor.SetActive(true);
    }

    /**
     * Fonction qui cache l'infobulle
     * @param void
     * @return void
     */
    public void CacherToolTip()
    {
        tooltipTexte.text = "";
        StopAllCoroutines();
        tooltipAnchor.SetActive(false);
    }

    /**
     * Fonction de limitation de la position de l'infobulle pour quelle reste à l'intérieur de l'écran
     * @param void
     * @return void
     */
    void ClampToolTip()
    {
        Vector3 pos = tooltipAnchorRectTransform.localPosition;

        Vector3 positionMin = tooltipParentRectTransform.rect.min - tooltipAnchorRectTransform.rect.min;
        Vector3 positionMax = tooltipParentRectTransform.rect.max - tooltipAnchorRectTransform.rect.max;

        pos.x = Mathf.Clamp(tooltipAnchorRectTransform.localPosition.x, positionMin.x, positionMax.x);
        pos.y = Mathf.Clamp(tooltipAnchorRectTransform.localPosition.y, positionMin.y, positionMax.y);

        tooltipAnchorRectTransform.localPosition = pos;
    }
}