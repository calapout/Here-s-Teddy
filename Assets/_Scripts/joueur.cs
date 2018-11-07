using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class joueur : MonoBehaviour {

    public float forceSaut;
    public float multiplicateurGravite;
    public float multiplicateurSautMin;
    public float vitesseDeplacement;
    public float distanceRaycast;
    public AudioClip SonSaut;

    private Transform _arme;
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
        _arme = gameObject.transform.GetChild(3);
    }
	
	// Update is called once per frame
	void Update () {
        /******************************************************déplacements************************************************************/
        float deplacementHorizontale = Input.GetAxisRaw("Horizontal");
        //float deplacementVerticale = Input.GetAxisRaw("Vertical");
        Vector3 deplacement = Vector3.zero;
        deplacement.x = deplacementHorizontale * vitesseDeplacement;
        _RB.velocity = deplacement;

        /******************************************************saut**********************************************************************/

        //détection du saut
        if (Input.GetButton("Jump") && _enLair == false)
        {
            _RB.velocity = Vector3.up * forceSaut;
            _AS.PlayOneShot(SonSaut, 0.5f);
            _animator.SetBool("saut", true);
            _enLair = true;
        }

        //gestion du saut
        if (_enLair == true)
        {
            // Gestion du saut
            if (_RB.velocity.y < 0)
            {
                _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
            }
            else if (_RB.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurSautMin - 1) * Time.deltaTime;
            }
        }

        /******************************************************Orientation*****************************************************************/
        if (_RB.velocity.x < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (_RB.velocity.x > 0)
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
            if (debugRaycast) Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down) * raycast.distance, Color.red);
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
            _enLair = true;
        }

        /**************************************************combat*******************************************************************************/
        if (Input.GetButton("Gauche") || Input.GetButton("Droite"))
        {
            Debug.Log("yolo");
            _animator.SetTrigger("attaque");
            _arme.gameObject.SetActive(true);
        }
    }



    
}
