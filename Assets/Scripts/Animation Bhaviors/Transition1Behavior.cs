using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition1Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical")==0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            PlayerAttack.attackInstance.anim.Play("Jab 2 Start");
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && PlayerAttack.attackInstance.anim.GetBool("Idle")==true && !PlayerAttack.attackInstance.playerMovement.isInAir && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Up Tilt");
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Vertical") < 0 && PlayerAttack.attackInstance.anim.GetBool("Crouch") && !PlayerAttack.attackInstance.playerMovement.isInAir&& !PlayerAttack.attackInstance.isGrab)
        {
            PlayerAttack.attackInstance.anim.Play("DTilt");
        }

        else if (Input.GetButtonDown("Fire1") && Input.GetAxisRaw("Horizontal") != 0 && !PlayerAttack.attackInstance.playerMovement.isInAir && !PlayerAttack.attackInstance.anim.GetBool("Running") && !PlayerAttack.attackInstance.isGrab)
        {
            PlayerAttack.attackInstance.anim.Play("Forward Tilt");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttack.attackInstance.isAttacking = false;
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
