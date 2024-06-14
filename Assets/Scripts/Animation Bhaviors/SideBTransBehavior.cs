using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBTransBehavior : StateMachineBehaviour
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
        PR.Attack.isAttacking = false;
        if(PR.Move.isInAir)
        {
            animator.SetBool("isGrounded", false);
            Physics2D.gravity = new Vector2(0 , -9.81f);
        } 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PR = PlayerRefrecnces.instance;
        if (PR.anim.GetCurrentAnimatorStateInfo(0).IsName("Single Jump Fall"))
        {
           PR.RB.gravityScale =PR.Move.scaledGravity;

            if(PR.Attack.isInHelpless)
               PR.Move.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
        }

        PR.anim.SetBool("isJumping", false);
        PR.anim.SetBool("isDoubleJumping", false);
        PR.Attack.isAttacking = false;
        PR.Attack.isSpecial = false;
        PR.Attack.bypassMoveBlock = false;
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
