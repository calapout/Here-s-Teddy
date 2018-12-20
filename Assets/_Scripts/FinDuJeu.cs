using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * FinDuJeu.cs
 * Script de gestion de l'affichage de la scène de fin selon l'état de la partie
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class FinDuJeu : MonoBehaviour {

    public Image fond; //Image de fond de la scène
    public Sprite fondPerdu; //Image si le joueur est mort
    public TextMeshProUGUI texte; //Texte de réussite ou de mort
    public GameObject generique; //Générique de fin si le joueur gagne
    public Transform boutons; //Parent des boutons

    GameObject btnContinuer; //Bouton Continuer
    bool _aGagner; //Indique si le joueur à gagné

    /**
     * Fonction d'initialisation des variables et de modification de l'affichage de la scène
     * @param void
     * @return void
     */
    void Start () {
        btnContinuer = boutons.GetChild(1).gameObject;
        _aGagner = ConditionDeFin.Instance.aGagner;
        Destroy(ConditionDeFin.Instance.gameObject); //Destruction de la variable static d'état du jeu (pour néttoyage entre les scènes)

        //Si le joueur n'a pas gagné...
        if (!_aGagner)
        {
            //Modifie l'affichage en conséquence
            fond.sprite = fondPerdu;
            texte.text = "Vous êtes mort. Voulez-vous continuer?";
            generique.SetActive(false);
            btnContinuer.SetActive(true);
        }
	}
}