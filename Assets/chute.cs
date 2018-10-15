using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chute : StateMachineBehaviour {

    private Rigidbody _RB;
    private GameObject _objet;

    public float vitesseDeplacement;
    public float multiplicateurGravite;
    public float distanceRaycast;

    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;
    public bool debugRaycast;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _objet = animator.gameObject;
        _RB = _objet.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        //Gestion de la détection avec le sol
        RaycastHit raycast;

        if (Physics.Raycast(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down), out raycast, distanceRaycast)) {
            animator.SetBool("chute", false);

            if (debugRaycast) Debug.DrawRay(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down) * raycast.distance, Color.yellow);
        }
        else {
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
        }

        Vector3 deplacement = _RB.velocity;
        deplacement.x = Input.GetAxisRaw("Horizontal") * vitesseDeplacement;

        _RB.velocity = deplacement;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    public void debug(bool x, bool y, bool z) {
        if (x) Debug.Log("x:" + _RB.velocity.x);
        if (y) Debug.Log("y:" + _RB.velocity.y);
        if (z) Debug.Log("z:" + _RB.velocity.z);
    }
}
