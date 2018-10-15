using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvement : StateMachineBehaviour {
    private Rigidbody _RB;
    private GameObject _objet;

    public float vitesseDeplacement;
    public float forceSaut;

    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _objet = animator.gameObject;
        _RB = _objet.GetComponent<Rigidbody>();
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
            animator.SetBool("saut", true);
        }

        animator.SetFloat("deplacement", _RB.velocity.x);

        if (debugVelocite) debug(debugX, debugY, debugZ);
    }

    public void debug(bool x, bool y, bool z){
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}