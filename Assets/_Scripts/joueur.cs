using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class joueur : MonoBehaviour {

    public float forceSaut = 0.005f;
    public float multiplicateurGravite;
    public float multiplicateurSautMin;
    public float vitesseDeplacement;
    public float distanceRaycast;
    public AudioClip SonSaut;
    public Transform arme;

    private Rigidbody _RB;
    private Animator _animator;
    private AudioSource _AS;
    public bool _enLair = false;


    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;
    public bool debugRaycast;

	// Use this for initialization
	void Start () {
        _RB = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _AS = Camera.main.gameObject.GetComponent<AudioSource>();
        arme = gameObject.transform.GetChild(3);
    }
	
	// Update is called once per frame
	void Update () {
        /******************************************************déplacements************************************************************/
        float deplacementHorizontale = Input.GetAxisRaw("Horizontal");
        //float deplacementVerticale = Input.GetAxisRaw("Vertical");
        Vector3 deplacement = _RB.velocity;
        deplacement.x = deplacementHorizontale * vitesseDeplacement;

        /******************************************************saut**********************************************************************/

        //détection du saut
        if (Input.GetButtonDown("Jump") && _enLair == false)
        {
            deplacement.y = forceSaut;
            _AS.PlayOneShot(SonSaut, 0.5f);
            //_animator.SetBool("saut", true);
        }

        // Gestion du saut
        if (_RB.velocity.y < 0)
        {
            deplacement.y += Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
        }
        else if (_RB.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            deplacement.y += Physics.gravity.y * (multiplicateurSautMin - 1) * Time.deltaTime;
        }

        /******************************************************Orientation*****************************************************************/
        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        /************************************************Gestion de la détection avec le sol************************************************/
        RaycastHit raycast;

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down), out raycast, distanceRaycast))
        {
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast.distance, Color.yellow);
            _enLair = false;
        }
        else
        {
            if (debugRaycast) {
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * distanceRaycast, Color.red);
            };
            _enLair = true;
        }

        /**************************************************combat*******************************************************************************/
        if (Input.GetButton("Gauche") || Input.GetButton("Droite"))
        {
            _animator.SetTrigger("attaque");
            arme.gameObject.SetActive(true);
        }




        /**********************************************************débug************************************************************************/
        if (debugVelocite) debug(debugX, debugY, debugZ);

        /**********************************************************déplacements*****************************************************************/
        _RB.velocity = deplacement;
    }


    public void debug(bool x, bool y, bool z)
    {
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}
