using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * gère la détection de la distance entre l'ennemi et teddy
 * @author Jimmy Tremblay-Bernier
 */
public class idleIA : StateMachineBehaviour {

    //variables
    public float distanceTrigger;
    private Transform _Teddy;

	 //lors de l'entré de l'état
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _Teddy = GameObject.Find("Teddy").transform;
	}

    // durant l'état
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Vector3.Distance(_Teddy.position, animator.gameObject.transform.position) < distanceTrigger) {
            animator.SetBool("TeddyEstProche", true);
        }
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
}
