using System.Collections;
using UnityEngine;
using TMPro;

public class ToolTipController : MonoBehaviour {

    public GameObject tooltipAnchor;
    TextMeshProUGUI tooltipTexte;
    IEnumerator CoroutineAffichage;

    RectTransform tooltipParentRectTransform;
    RectTransform tooltipAnchorRectTransform;

    public Vector3 tooltipOffset;

	void Start () {
        tooltipTexte = tooltipAnchor.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tooltipParentRectTransform = tooltipAnchor.transform.parent.GetComponent<RectTransform>();
        tooltipAnchorRectTransform = tooltipAnchor.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (tooltipAnchor.activeSelf)
        {
            tooltipAnchor.transform.position = Input.mousePosition + tooltipOffset;
            ClampToolTip();
        }
    }

    public void AfficherToolTip(string texte)
    {
        tooltipTexte.text = texte;
        CoroutineAffichage = Afficher();
        StartCoroutine(CoroutineAffichage);
    }

    IEnumerator Afficher()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        tooltipAnchor.SetActive(true);
    }

    public void CacherToolTip()
    {
        tooltipTexte.text = "";
        StopAllCoroutines();
        tooltipAnchor.SetActive(false);
    }

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