using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvement : StateMachineBehaviour {
    private Rigidbody _RB;
    private AudioSource _AP;
    private GameObject _objet;

    public AudioClip SonSaut;
    public float vitesseDeplacement;
    public float forceSaut;
    public float distanceRaycast;

    //trigger
    private bool aSauter = false;

    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _objet = animator.gameObject;
        _RB = _objet.GetComponent<Rigidbody>();
        _AP = Camera.main.GetComponent<AudioSource>();
        aSauter = false;
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float deplacementHorizontale = Input.GetAxisRaw("Horizontal");
        //float deplacementVerticale = Input.GetAxisRaw("Vertical");
        Vector3 deplacement = Vector3.zero;
        deplacement.x = deplacementHorizontale * vitesseDeplacement;
        _RB.velocity = deplacement;

        if (Input.GetButton("Jump")) {
            _RB.velocity = Vector3.up * forceSaut;
            if (aSauter == false) {
                _AP.PlayOneShot(SonSaut, 0.5f);
                aSauter = true;
            }
            animator.SetBool("saut", true);
        }

        RaycastHit raycast;

        if (!Physics.Raycast(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down), out raycast, distanceRaycast)) {
            animator.SetBool("chute", true);
        }

        animator.SetFloat("deplacement", _RB.velocity.x);

        if (_RB.velocity.x < 0)
        {
            _objet.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (_RB.velocity.x > 0)
        {
            _objet.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        if (debugVelocite) debug(debugX, debugY, debugZ);
    }

    public void debug(bool x, bool y, bool z){
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}