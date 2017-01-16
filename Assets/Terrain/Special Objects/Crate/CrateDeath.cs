using UnityEngine;
using System.Collections;

public class CrateDeath : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Transform crate = animator.transform;
		Destroy(crate.GetComponent<Rigidbody>());
		Destroy(crate.GetComponent<BoxCollider>());

		foreach (Transform side in crate) {
			Rigidbody rb = (Rigidbody) side.gameObject.AddComponent<Rigidbody>();
			rb.mass = 0.1f;
			rb.velocity = new Vector3(0f, Random.Range(-1f, 3f), 0f);

			side.GetComponent<BoxCollider>().enabled = true;
			side.GetComponent<CrateSide>().enabled = true;


			foreach (Transform renderSide in side) {
				renderSide.GetComponent<MeshRenderer>().material.color = Color.grey;
			}
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
