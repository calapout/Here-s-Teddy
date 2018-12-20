using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Continuer : MonoBehaviour {

    public void ContinuerPartie()
    {
        if (Directory.Exists("Assets/Resources/Saves"))
        {
            if (File.Exists("Assets/Resources/Saves/save.json"))
            {
                PlayerPrefs.SetInt("chargerSauvegarde", 1);
            }
            else
            {
                PlayerPrefs.SetInt("chargerSauvegarde", 0);
            }
            SceneManager.LoadScene("Chambre1");
        }
    }
}