using UnityEngine;

/**
 * ConditionDeFin.cs
 * Script d'indication de l'état de la partie
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class ConditionDeFin : MonoBehaviour {

    public static ConditionDeFin Instance { get; private set;} //Ref public et static à l'instance de ce script
    ConditionDeFin _instance; //Ref privée à l'instance de ce script
    [HideInInspector]
	public bool aGagner = false; //Indique si le joueur à gagner la partie

    /**
     * Fonction d'initialisation des l'instances de ce script
     * @param void
     * @return void
     */
    void Awake()
    {
        _instance = this;
        Instance = _instance;
        DontDestroyOnLoad(gameObject);
    }
}