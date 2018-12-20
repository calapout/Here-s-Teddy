using UnityEngine.SystemeEventsLib;
using UnityEngine;

/**
 * Rage.cs
 * Script de gestion de l'aquisition de la rage et de l'activation du mode Rage
 * @author Yoann Paquette
 * @version Mercredi 19 Décembre 2018
 */
public class Rage : MonoBehaviour {

    arme armeRef; //Ref à l'arme du joueur
    public GameObject effetRageRef; //Ref à l'effet du mode Rage (ParticleSystem)

    public int pointsDeRage = 0; //Points de rage
    public int pointsDeRageMax = 50; //Points de rage maximum
    public int duree = 10; //Durée du mode Rage
    [Tooltip("Gain de points de rage par Kill / Points dégâts subit.")]
    public Vector2 gain = new Vector2(1, 5); //Gain de rage selon la source
    public int multiplicateur = 2; //Multiplicateur de dégâts (quand Rage activé)

    int nbIterations = 0; //Nombre de fois qu'une fonction est activé (timer en secondes)
    int degatsTotalTmp; //Dégâts totaux que le joueur peut infliger en mode Rage

    bool estPlein = false; //La rage est-elle pleine?
    bool rageActiver = false; //Indicateur d'activation de la rage

    public Texture2D texTeddy; //Texture de Teddy normal
    public Texture2D texTeddyRage; //Texture de Teddy en mode Rage

    [HideInInspector]
    public enum TypeGain { Kill, Degats} //Type de gain de rage (gain par mort d'ennemi, ou par dégâts subits)

    InfoEvent rageInfoEvent = new InfoEvent();

    /**
     * Fonction d'initialisation de la référence de l'arme
     * @param void
     * @return void
     */
    void Start () {
		armeRef = transform.GetChild(3).GetComponent<arme>();
    }

    /**
     * Fonction de vérification d'entrée clavier par le joueur et d'activation du mode Rage
     * @param void
     * @return void
     */
    void Update () {
        //Si la touche H est appuyé, que la rage n'est pas activé et qu'elle est pleine...
        if (Input.GetKeyDown(KeyCode.H) && !rageActiver && estPlein)
        {
            //Activer le mode Rage
            rageActiver = true;
            estPlein = false;
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTexture = texTeddyRage;
            effetRageRef.SetActive(true);
            InvokeRepeating("ActiverRage", 0, 1);
        }
	}

    /**
     * Fonction d'ajout de rage selon le type
     * @param enum TypeGain type (Kill, Degats)
     * @return int (Points de rage gagnés)
     */
    public int GainRage(TypeGain type)
    {
        float tmp = pointsDeRage;
        //Si la rage n'est pas activé et qu'elle n'est pas pleine...
        if (!rageActiver && !estPlein)
        {
            //Ajout de rage selon le type de la source
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

    /**
     * Fonction qui vérifie et qui retourne l'état de la rage
     * @param float rage (Points de rage du joueur)
     * @return bool (false = non remplit, true = remplit)
     */
    bool VerificationRemplissage(float rage)
    {
        bool tmp = false;
        //Si la rage a atteint son maximum...
        if (rage >= pointsDeRageMax)
        {
            //Return true, sinon return false
            tmp = true;
        }
        return tmp;
    }

    /**
     * Fonction qui initialise les paramètres un événement RageEventSetup et lance celui-ci
     * @param void
     * @return void
     */
    public void RageEventSetup()
    {
        rageInfoEvent.Rage = pointsDeRage;
        rageInfoEvent.RageMax = pointsDeRageMax;
        SystemeEvents.Instance.LancerEvent(NomEvent.updateUiRageEvent, rageInfoEvent); //Lancement de l'event
    }

    /**
     * Fonction d'activation et de gestion de la rage
     * @param void
     * @return void
     */
    void ActiverRage()
    {
        //Si le mode rage vient d'être lancé...
        if (nbIterations == 0)
        {
            //Modifie certains paramètres
            degatsTotalTmp = armeRef.degatsTotal;
            armeRef.degatsTotal *= multiplicateur;
        }
        
        pointsDeRage -= pointsDeRageMax / duree;
        RageEventSetup();

        //Si le nombre d'itérations atteint la durée du mode Rage...
        if (nbIterations >= duree - 1)
        {
            //Désactive le mode Rage est réinitialise certains paramètres
            armeRef.degatsTotal = degatsTotalTmp;
            rageActiver = false;
            nbIterations = 0;
            effetRageRef.SetActive(false);
            transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material.mainTexture = texTeddy;
            CancelInvoke("ActiverRage");
        }
        else
        {
            //Sinon, continuation de l'éxécution
            nbIterations++;
        }
    }
}