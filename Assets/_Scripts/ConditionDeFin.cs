using UnityEngine;

public class ConditionDeFin : MonoBehaviour {

    public static ConditionDeFin Instance { get; private set;}
    ConditionDeFin _instance;
	public bool aGagner = false;

    void Awake()
    {
        _instance = this;
        Instance = _instance;
        DontDestroyOnLoad(gameObject);
    }
}