using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") == 0 && PlayerAttack.attackInstance.anim.GetBool("Idle") == true && !PlayerAttack.attackInstance.playerMovement.isInAir && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Jab 1 Start");
        }

        else if (PlayerAttack.attackInstance.isAttacking && PlayerAttack.attackInstance.anim.GetBool("Idle") == true && !PlayerAttack.attackInstance.playerMovement.isInAir && PlayerAttack.attackInstance.isSpecial && Input.GetAxisRaw("Vertical")==0)
        {
            PlayerAttack.attackInstance.anim.SetBool("isSticked", PlayerAttack.attackInstance.isSticked);
            PlayerAttack.attackInstance.anim.Play("Neutral B Start");
            PlayerAttack.attackInstance.stickyHand.SetActive(true);
        }

        else if(PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") > 0f && !PlayerAttack.attackInstance.playerMovement.isInAir && PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("USpecial");
        }

        else if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical") > 0 && PlayerAttack.attackInstance.anim.GetBool("Idle")==true && !PlayerAttack.attackInstance.playerMovement.isInAir)
        {
            Debug.Log("Called!");
            PlayerAttack.attackInstance.anim.Play("Up Tilt");
        }

        else if (string.Equals(PlayerTaunt.TauntInstance.taunt , "Up") && Input.GetAxisRaw("Vertical") == 0 && PlayerTaunt.TauntInstance.anim.GetBool("Idle") == true && !PlayerTaunt.TauntInstance.PM.isInAir)
        {
            PlayerTaunt.TauntInstance.anim.Play("UpTaunt");
        }

        else if (string.Equals(PlayerTaunt.TauntInstance.taunt , "Down") && Input.GetAxisRaw("Vertical") == 0 && PlayerTaunt.TauntInstance.anim.GetBool("Idle") == true && !PlayerTaunt.TauntInstance.PM.isInAir)
        {
            PlayerTaunt.TauntInstance.anim.Play("DownTaunt");
        }

        else if (string.Equals(PlayerTaunt.TauntInstance.taunt , "Left") && Input.GetAxisRaw("Vertical") == 0 && PlayerTaunt.TauntInstance.anim.GetBool("Idle") == true && !PlayerTaunt.TauntInstance.PM.isInAir)
        {
            PlayerTaunt.TauntInstance.anim.Play("LeftTaunt");
        }

        else if (string.Equals(PlayerTaunt.TauntInstance.taunt , "Up") && Input.GetAxisRaw("Vertical") == 0 && PlayerTaunt.TauntInstance.anim.GetBool("Idle") == true && !PlayerTaunt.TauntInstance.PM.isInAir)
        {
            PlayerTaunt.TauntInstance.anim.Play("RightTaunt");
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!PlayerAttack.attackInstance.stickyHand.activeSelf)
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
