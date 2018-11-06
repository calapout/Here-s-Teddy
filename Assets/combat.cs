using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : StateMachineBehaviour {
    public Transform arme;
    public float vitesseDeplacement;
    public float multiplicateurGravite;
    public float multiplicateurSautMin;

    private GameObject _objet;
    private Rigidbody _RB;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _objet = animator.gameObject;
        _RB = _objet.GetComponent<Rigidbody>();
        arme = _objet.transform.GetChild(3);
        arme.gameObject.SetActive(true);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    float deplacementHorizontale = Input.GetAxisRaw("Horizontal");
        //float deplacementVerticale = Input.GetAxisRaw("Vertical");
        Vector3 deplacement = Vector3.zero;
        deplacement.x = deplacementHorizontale * vitesseDeplacement;
        _RB.velocity = deplacement;

        // Gestion du saut
        if (_RB.velocity.y < 0) {
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
        }
        else if (_RB.velocity.y > 0 && !Input.GetButton("Jump")) {
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurSautMin - 1) * Time.deltaTime;
        }


        if (_RB.velocity.x < 0) {
            _objet.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (_RB.velocity.x > 0) {
            _objet.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        arme.gameObject.SetActive(false);

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
