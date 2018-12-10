using UnityEngine.SystemeEventsLib;
using UnityEngine;

public class Rage : MonoBehaviour {

    arme armeRef;
    public GameObject effetRageRef;

    public int pointsDeRage = 0;
    public int pointsDeRageMax = 50;
    public int duree = 10;
    [Tooltip("Gain de points de rage par Kill / Points dégâts subit.")]
    public Vector2 gain = new Vector2(1, 5);
    public int multiplicateur = 2;

    int nbIterations = 0;
    int degatsTotalTmp;

    bool estPlein = false;
    bool rageActiver = false;

    public Texture2D texTeddy;
    public Texture2D texTeddyRage;

    [HideInInspector]
    public enum TypeGain { Kill, Degats}

    InfoEvent rageInfoEvent = new InfoEvent();

    void Start () {
		armeRef = transform.GetChild(3).GetComponent<arme>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.H) && !rageActiver && estPlein)
        {
            rageActiver = true;
            estPlein = false;
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTexture = texTeddyRage;
            effetRageRef.SetActive(true);
            InvokeRepeating("ActiverRage", 0, 1);
        }
	}

    public int GainRage(TypeGain type)
    {
        float tmp = pointsDeRage;
        if (!rageActiver && !estPlein)
        {
            if (type == TypeGain.Kill)
            {
                tmp += gain.x;
            }
            else if (type == TypeGain.Degats)
            {
                tmp += gain.y;
            }
            tmp = Mathf.Clamp(tmp, 0, pointsDeRageMax);

            estPlein = VerificationRemplissage(tmp);
        }
        return (int)tmp;
    }

    bool VerificationRemplissage(float rage)
    {
        bool tmp = false;
        if (rage >= pointsDeRageMax)
        {
            tmp = true;
        }
        return tmp;
    }

    public void RageEventSetup()
    {
        rageInfoEvent.Rage = pointsDeRage;
        rageInfoEvent.RageMax = pointsDeRageMax;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiRageEvent, rageInfoEvent);
    }

    void ActiverRage()
    {
        if (nbIterations == 0)
        {
            degatsTotalTmp = armeRef.degatsTotal;
            armeRef.degatsTotal *= multiplicateur;
        }
        
        pointsDeRage -= pointsDeRageMax / duree;
        RageEventSetup();

        if (nbIterations >= duree - 1)
        {
            armeRef.degatsTotal = degatsTotalTmp;
            rageActiver = false;
            nbIterations = 0;
            effetRageRef.SetActive(false);
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTexture = texTeddy;
            CancelInvoke("ActiverRage");
        }
        else
        {
            nbIterations++;
        }
    }
}