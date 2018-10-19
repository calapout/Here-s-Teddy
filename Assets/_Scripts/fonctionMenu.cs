using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class fonctionMenu : MonoBehaviour {
    public string NiveauACharger;

    public void jouer() {
        SceneManager.LoadScene(NiveauACharger);
    }

    //Non complet pour L'alpha
    public void options() {

    }

    public void quitter() {
        Application.Quit();
    }
}
