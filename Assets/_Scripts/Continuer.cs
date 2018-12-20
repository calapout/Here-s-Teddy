using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/**
 * Continuer.cs
 * Script de gestion de la continuation de la partie quand le bouton Continuer est appuyé
 * @author Yoann PAquette
 * @version Mercredi 19 Décembre 2018
 */
public class Continuer : MonoBehaviour {

    /**
     * Fonction de vérification d'existence d'une sauvegarde avant de la charger
     * @param void
     * @return void
     */
    public void ContinuerPartie()
    {
        //Si le dossier de sauvegardes existe...
        if (Directory.Exists("Assets/Resources/Saves"))
        {
            //Si le fichier de sauvegarde existe...
            if (File.Exists("Assets/Resources/Saves/save.json"))
            {
                //Charge le fichier
                PlayerPrefs.SetInt("chargerSauvegarde", 1);
            }
            else
            {
                //Continue sans charger de fichier
                PlayerPrefs.SetInt("chargerSauvegarde", 0);
            }
            SceneManager.LoadScene("Chambre1");
        }
    }
}