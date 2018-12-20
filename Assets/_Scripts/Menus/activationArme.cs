using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gère l'affichages des armes dans le menu en jeu.
/// </summary>
/// <remarks> Auteur: Jimmy Tremblay-Bernier</remarks>
public class activationArme : MonoBehaviour {

    private GameObject teddy; //référence à Teddy
    private Transform panel; //référence au panneau

    private void OnEnable() {
        teddy = GameObject.Find("Teddy");
        List<ArmeTemplate> listeArme = new List<ArmeTemplate>();

        //on récupère la liste des armes
        listeArme = teddy.GetComponent<joueur>().inventaireArmeTemplates;

        panel = gameObject.transform.GetChild(1);

        //On passe à travers chaque arme et on l'affiche dans le menu
        for (int i = 0; i < listeArme.Count; i++) {
            Transform enfant = panel.GetChild(i);
            enfant.gameObject.name = listeArme[i].nom;
            enfant.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
            enfant.GetChild(0).GetChild(0).gameObject.name = listeArme[i].nom;
            enfant.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(listeArme[i].nom);
            enfant.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().SetText("Dégats: "+ listeArme[i].degat);
        }
    }

    //Lors du clique sur le bouton, permet d'aller chercher le nom de l'arme correspondant à l'index et effectue le changement d'arme en conséquant
    public void SelectionnerArme(int noArme) {
        Debug.Log(noArme);
        teddy.GetComponent<joueur>().ChangementArme(teddy.GetComponent<joueur>().inventaireArme[noArme]);
        this.transform.parent.parent.gameObject.GetComponent<ouvrirMenu>().OuvrirPanelArmes();
    }
}
