using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltBehavior : StateMachineBehaviour
{
    PlayerRefrecnces PR;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PR = PlayerRefrecnces.instance;
        if (!(PR.anim.GetCurrentAnimatorStateInfo(0).IsName("Up Tilt") || PR.anim.GetCurrentAnimatorStateInfo(0).IsName("Up Tilt Transition")))
        {

            if (Input.GetAxisRaw("Vertical") > 0 && PR.anim.GetBool("Idle") == true && !PR.Move.isInAir && !PR.Attack.isSpecial)
            {
                PR.anim.Play("Up Tilt");
            }
        }

        if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") < 0 && PR.anim.GetBool("Crouch") && !PR.Move.isInAir && !PR.Attack.isGrab)
        {
            PR.anim.Play("DTilt");
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Horizontal") != 0 && !PR.Move.isInAir && !PR.anim.GetBool("Running") && !PR.Attack.isGrab)
        {
            PR.anim.Play("Forward Tilt");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PR = PlayerRefrecnces.instance;
        PR.Attack.isAttacking = false;
    }


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
