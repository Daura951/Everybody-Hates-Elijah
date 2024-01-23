using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBTransBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerAttack.attackInstance.isAttacking = false;
        if(PlayerAttack.attackInstance.playerMovement.isInAir)
        {
            animator.SetBool("isGrounded", false);
            Physics2D.gravity = new Vector2(0 , -9.81f);
        } 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAttack.attackInstance.playerMovement.GetAnim().GetCurrentAnimatorStateInfo(0).IsName("Single Jump Fall"))
        {
            PlayerAttack.attackInstance.playerMovement.rb.gravityScale = PlayerAttack.attackInstance.playerMovement.scaledGravity;

            if(PlayerAttack.attackInstance.isInHelpless)
                PlayerAttack.attackInstance.playerMovement.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
        }

        PlayerAttack.attackInstance.playerMovement.anim.SetBool("isJumping", false);
        PlayerAttack.attackInstance.playerMovement.anim.SetBool("isDoubleJumping", false);
        PlayerAttack.attackInstance.isAttacking = false;
        PlayerAttack.attackInstance.isSpecial = false;
        PlayerAttack.attackInstance.bypassMoveBlock = false;
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
