using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    PlayerRefrecnces PR;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PR = PlayerRefrecnces.instance;
        foreach (GameObject hitBox in PR.Attack.hitBoxes)
        {
            hitBox.SetActive(false);
        }
        PR.Move.isInGetup = false;
        PR.anim.SetBool("isLaying", false);
        PR.anim.SetBool("canLayAttack", false);
        PR.Move.Speed = 9f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PR.Attack.isAttacking && Input.GetAxisRaw("Vertical") == 0 && PR.anim.GetBool("Idle") == true && !PR.Move.isInAir && !PR.Attack.isSpecial && !PR.Attack.isGrab) //&& !PR.Attack.isBladeBound)
        {
            PR.anim.Play("Jab 1 Start");
        }

        else if (PR.Attack.isAttacking && PR.anim.GetBool("Idle") == true && !PR.Move.isInAir && PR.Attack.isSpecial && Input.GetAxisRaw("Vertical")==0 && !PR.Attack.isBladeBound)
        {
            PR.anim.SetBool("isSticked", PR.Attack.isSticked);
            PR.anim.Play("Neutral B Start");
            PR.Attack.stickyHand.SetActive(true);
        }

        else if (PR.Attack.isAttacking && PR.anim.GetBool("Idle") == true && !PR.Move.isInAir && Input.GetAxisRaw("Vertical") == 0 && PR.Attack.isBladeBound && PR.Attack.isSpecial)
        {
            PR.anim.Play("BladeBoundActivation");
        }

        else if(PR.Attack.isAttacking && Input.GetAxisRaw("Vertical") > 0f && PR.Attack.isSpecial)
        {
            PR.anim.Play("USpecial");
        }

        else if (PR.Attack.isAttacking && Input.GetAxisRaw("Vertical") > 0 && PR.anim.GetBool("Idle")==true && !PR.Move.isInAir && !PR.Attack.isSpecial)
        {
            PR.anim.Play("Up Tilt");
        }

        else if(PR.Attack.isAttacking && Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0 && PR.Attack.isGrab && !PR.Attack.isSpecial)
        {
            PR.anim.Play("Grabbing");
        }

        else if (string.Equals(PR.Taunt.taunt , "Up") && Input.GetAxisRaw("Vertical") == 0 && PR.Taunt.anim.GetBool("Idle") == true && !PR.Move.isInAir)
        {
            PR.Taunt.anim.Play("UpTaunt");
        }

        else if (string.Equals(PR.Taunt.taunt , "Down") && Input.GetAxisRaw("Vertical") == 0 && PR.Taunt.anim.GetBool("Idle") == true && !PR.Move.isInAir)
        {
            PR.Taunt.anim.Play("DownTaunt");
        }

        else if (string.Equals(PR.Taunt.taunt , "Left") && Input.GetAxisRaw("Vertical") == 0 && PR.Taunt.anim.GetBool("Idle") == true && !PR.Move.isInAir)
        {
            PR.Taunt.anim.Play("LeftTaunt");
        }

        else if (string.Equals(PR.Taunt.taunt , "Right") && Input.GetAxisRaw("Vertical") == 0 && PR.Taunt.anim.GetBool("Idle") == true && !PR.Move.isInAir)
        {
            PR.Taunt.anim.Play("RightTaunt");
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PR = PlayerRefrecnces.instance;
        if (!PR.Attack.stickyHand.activeSelf)
        {
            PR.Attack.isAttacking = false;
        }
            
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
