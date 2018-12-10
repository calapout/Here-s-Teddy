using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class fonctionMenu : MonoBehaviour {
    public string NiveauACharger;

    public GameObject ConfirmationMenu;
    public GameObject avecSauvegarde;
    public GameObject sansSauvegarde;

    private void Start() {
        ConfirmationMenu.SetActive(false);
        if (System.IO.Directory.Exists("Assets/Resources/Saves")) {
            if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
                avecSauvegarde.SetActive(true);
                sansSauvegarde.SetActive(false);
            }
            else {
                avecSauvegarde.SetActive(false);
                sansSauvegarde.SetActive(true);
            }
        }
        else {
            System.IO.Directory.CreateDirectory("Assets/Resources/Saves");
            avecSauvegarde.SetActive(false);
            sansSauvegarde.SetActive(true);
        }

    }

    public void NouvellePartie() {
        if (System.IO.File.Exists("Assets/Resources/Saves/save.json")) {
            ConfirmationMenu.SetActive(true);
        }
        else {
            ValiderNouvellePartie();
        }
    }

    public void ValiderNouvellePartie() {
        System.IO.File.Delete("Assets/Resources/Saves/save.json");
        PlayerPrefs.SetInt("chargerSauvegarde", 0);
        SceneManager.LoadScene(NiveauACharger);
    }

    public void AnnulerNouvellePartie() {
        ConfirmationMenu.SetActive(false);
    }

    public void Continuer() {
        PlayerPrefs.SetInt("chargerSauvegarde", 1);
        SceneManager.LoadScene(NiveauACharger);
    }

    //Non complet pour L'alpha
    public void Options() {

    }

    public void Quitter() {
        Application.Quit();
    }
}
