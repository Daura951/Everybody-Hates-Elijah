using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleJumpBheavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            PlayerAttack.attackInstance.anim.Play("Nair");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        else if(PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") > 0 && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Uair");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        else if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") < 0 && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Dair");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        else if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Horizontal") != 0 && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Fair");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        else if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") > 0f && PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("USpecial");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        else if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical")< 0f && PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Down B");
            PlayerAttack.attackInstance.isAttacking = false;
        }

        if (PlayerAttack.attackInstance.isAttacking && PlayerAttack.attackInstance.isSpecial && Input.GetAxisRaw("Vertical")==0 && Input.GetAxisRaw("Horizontal")!=0)
        {
            PlayerAttack.attackInstance.anim.Play("Side B");
            PlayerAttack.attackInstance.isAttacking = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
 
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
