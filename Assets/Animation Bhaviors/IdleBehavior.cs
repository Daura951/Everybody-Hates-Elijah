using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAttack.attackInstance.isAttacking && PlayerAttack.attackInstance.anim.GetBool("Idle") == true && !PlayerAttack.attackInstance.playerMovement.isInAir && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Jab 1 Start");
        }

        else if (PlayerAttack.attackInstance.isAttacking && PlayerAttack.attackInstance.anim.GetBool("Idle") == true && !PlayerAttack.attackInstance.playerMovement.isInAir && PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.SetBool("isSticked", PlayerAttack.attackInstance.isSticked);
            PlayerAttack.attackInstance.anim.Play("Neutral B Start");
            PlayerAttack.attackInstance.stickyHand.SetActive(true);
            Debug.Log("Play me!");
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
