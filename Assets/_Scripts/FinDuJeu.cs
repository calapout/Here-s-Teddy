using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinDuJeu : MonoBehaviour {

    public Image fond;
    public Sprite fondPerdu;
    public TextMeshProUGUI texte;
    public GameObject generique;
    public Transform boutons;

    GameObject btnContinuer;
    bool _aGagner;

	void Start () {
        btnContinuer = boutons.GetChild(1).gameObject;
        _aGagner = ConditionDeFin.Instance.aGagner;
        Destroy(ConditionDeFin.Instance.gameObject);

        if (!_aGagner)
        {
            fond.sprite = fondPerdu;
            texte.text = "Vous êtes mort. Voulez-vous continuer?";
            generique.SetActive(false);
            btnContinuer.SetActive(true);
        }
	}
}