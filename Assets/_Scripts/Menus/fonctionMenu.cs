using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
/// <remarks>Auteurs: Jimmy Tremblay-Bernier et Yoann Paquette</remarks>
public class fonctionMenu : MonoBehaviour {

    public string NiveauACharger; //le niveau à charger
    public GameObject ConfirmationMenu; //le panneau de confirmation
    public GameObject avecSauvegarde; //permet de lancer le jeu avec une sauvegarde
    public GameObject sansSauvegarde; //permet de lancer le jeu en supprimant la sauvegarde
    public GameObject panelControles; //panneau de contrôle

    /// <summary>
    /// Détecte si il y a présence d'une sauvegarde dans le dossier Resources/Saves
    /// </summary>
    private void Start() {
        ConfirmationMenu.SetActive(false);
        //Si le dossier existe
        if (System.IO.Directory.Exists("Assets/Resources/Saves")) {
            //si la sauvegarde existe
            if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
                avecSauvegarde.SetActive(true);
                sansSauvegarde.SetActive(false);
            }
            //si elle n'existe pas
            else {
                avecSauvegarde.SetActive(false);
                sansSauvegarde.SetActive(true);
            }
        }
        //si le dossier n'existe pas on en créer un
        else {
            System.IO.Directory.CreateDirectory("Assets/Resources/Saves");
            avecSauvegarde.SetActive(false);
            sansSauvegarde.SetActive(true);
        }

    }

    /// <summary>
    /// Si une sauvegarde existe, on ouvre le panneau de confirmation, sinon on lance la partie
    /// </summary>
    public void NouvellePartie() {
        if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
            ConfirmationMenu.SetActive(true);
        }
        else {
            ValiderNouvellePartie();
        }
    }

    /// <summary>
    /// Détruit la sauvegarde sans possibilité de retour. Un peu comme le petit frère qui détruit ta sauvegarde dans pokémon sans te le demander.
    /// Ensuite on lance la scène.
    /// </summary>
    public void ValiderNouvellePartie() {
        System.IO.File.Delete("Assets/Resources/Saves/save.json");
        PlayerPrefs.SetInt("chargerSauvegarde", 0);
        SceneManager.LoadScene(NiveauACharger);
    }

    /// <summary>
    /// Ferme le panneau de confirmation.
    /// </summary>
    public void AnnulerNouvellePartie() {
        ConfirmationMenu.SetActive(false);
    }

    /// <summary>
    /// Charge la partie en conservant la sauvegarde
    /// </summary>
    public void Continuer() {
        PlayerPrefs.SetInt("chargerSauvegarde", 1);
        SceneManager.LoadScene(NiveauACharger);
    }

    /// <summary>
    /// affiche les contrôles du jeu
    /// </summary>
    public void AfficherControles() {
        panelControles.SetActive(!panelControles.activeSelf);
    }

    /// <summary>
    /// Quitte le jeu
    /// </summary>
    public void Quitter() {
        Application.Quit();
    }
}
