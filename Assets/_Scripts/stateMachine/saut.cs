using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saut : StateMachineBehaviour {
    private Rigidbody _RB;
    private GameObject _objet;

    public float multiplicateurGravite;
    public float multiplicateurSautMin;
    public float distanceRaycast;
    public float vitesseDeplacementSaut;

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

        if (debugVelocite) debug(debugX, debugY, debugZ);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Gestion du saut
        if (_RB.velocity.y < 0) {
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurGravite - 1) * Time.deltaTime;
        }
        else if (_RB.velocity.y > 0 && !Input.GetButton("Jump")) {
            _RB.velocity += Vector3.up * Physics.gravity.y * (multiplicateurSautMin - 1) * Time.deltaTime;
        }

        Vector3 deplacement = _RB.velocity;
        deplacement.x = Input.GetAxisRaw("Horizontal") * vitesseDeplacementSaut;

        _RB.velocity = deplacement;

        //Gestion de la détection avec le sol
        RaycastHit raycast;

        if (Physics.Raycast(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down), out raycast, distanceRaycast)) {
            animator.SetBool("saut", false);

            if (debugRaycast) Debug.DrawRay(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down) * raycast.distance, Color.yellow);
        }
        else {
            if (debugRaycast) Debug.DrawRay(_objet.transform.position, _objet.transform.TransformDirection(Vector3.down) * distanceRaycast, Color.red);
        }


        animator.SetFloat("deplacement", _RB.velocity.x);

        if (debugVelocite)debug(debugX, debugY, debugZ);
    }

    public void debug (bool x, bool y, bool z){
        if(x)Debug.Log("x:" + _RB.velocity.x);
        if(y)Debug.Log("y:" + _RB.velocity.y);
        if (z)Debug.Log("z:" + _RB.velocity.z);
    }
}