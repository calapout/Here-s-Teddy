using UnityEngine;
using UnityEngine.UI;

public class VieEnnemiUpdate : MonoBehaviour {

    public Canvas barreVie;
    Image vieUi;
    Ennemi scriptEnnemi;
    float tmpVie;
    float vieMax;

    void Start () {
        vieUi = barreVie.transform.GetChild(1).GetComponent<Image>();
        scriptEnnemi = GetComponent<Ennemi>();
        tmpVie = (float)scriptEnnemi.pointsVie;
        vieMax = (float)scriptEnnemi.pointsVieMax;
	}
	
	void Update () {
		if (scriptEnnemi.pointsVie != tmpVie)
        {
            tmpVie = scriptEnnemi.pointsVie;
            if (!barreVie.gameObject.activeSelf)
            {
                barreVie.gameObject.SetActive(true);
            }
            vieUi.fillAmount = Mathf.Lerp(0f, 1f, tmpVie / vieMax);
        }
	}
}
