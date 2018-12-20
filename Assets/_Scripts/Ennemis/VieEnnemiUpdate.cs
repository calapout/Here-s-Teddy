using UnityEngine;
using UnityEngine.UI;

/**
 * VieEnnemiUpdate.cs
 * Script de mise à jour visuel de la barre de vie des ennemies
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class VieEnnemiUpdate : MonoBehaviour {

    public Canvas barreVie; //Barre de vie de l'ennemi (Canvas)
    Image vieUi; //Barre de vie de l'ennemi (Slider)
    Ennemi scriptEnnemi; //Ref au script de l'ennemi
    float tmpVie; //Valeur temporaire pour la vie
    float vieMax; //Vie maximale

    /**
     * Fonction d'initialisation des variables
     * @param void
     * @return void
     */
    void Start () {
        vieUi = barreVie.transform.GetChild(1).GetComponent<Image>();
        scriptEnnemi = GetComponent<Ennemi>();
        //Conversion (Cast) nécessaire pour que l'affichage d'update
        tmpVie = (float)scriptEnnemi.pointsVie;
        vieMax = (float)scriptEnnemi.pointsVieMax;
	}

    /**
     * Fonction de vérification des points de vie de l'ennemi et de modification de l'affichage
     * @param void
     * @return void
     */
    void Update () {
        //Si les points de vie actuel sont modifiés...
		if (scriptEnnemi.pointsVie != tmpVie)
        {
            //Change la variable temp
            tmpVie = scriptEnnemi.pointsVie;
            //Si la barre de vie n'est pas activé...
            if (!barreVie.gameObject.activeSelf)
            {
                //Activation de celle-ci
                barreVie.gameObject.SetActive(true);
            }
            vieUi.fillAmount = Mathf.Lerp(0f, 1f, tmpVie / vieMax);
        }
	}
}