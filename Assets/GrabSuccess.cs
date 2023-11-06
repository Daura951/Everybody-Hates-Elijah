using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSuccess : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Vertical")==0 && Input.GetAxisRaw("Horizontal")==0 &&PlayerAttack.attackInstance.anim.GetBool("hasGrabbedEnemy") == true && !PlayerAttack.attackInstance.playerMovement.isInAir && !PlayerAttack.attackInstance.isSpecial)
        {
            PlayerAttack.attackInstance.anim.Play("Pummel");
            PlayerAttack.attackInstance.isAttacking = false;

        }

        if(Input.GetAxisRaw("Vertical") > 0 && PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Horizontal") == 0)
        {
            PlayerAttack.attackInstance.anim.Play("UThrow");
            PlayerAttack.attackInstance.isAttacking = false;
            SetThrow();
            PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[0] = true;
        }

        if(Input.GetAxisRaw("Vertical") < 0 && PlayerAttack.attackInstance.isAttacking && Input.GetAxisRaw("Horizontal") == 0 )
        {
            PlayerAttack.attackInstance.anim.Play("DThrow");
            SetThrow();
            PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[3] = true;
        }

        if(Input.GetAxisRaw("Vertical") == 0 && PlayerAttack.attackInstance.isAttacking && ((PlayerAttack.attackInstance.playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (PlayerAttack.attackInstance.playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)))
        {
            PlayerAttack.attackInstance.anim.Play("FThrow");
            SetThrow();
            PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[1] = true;
        }

        if(Input.GetAxisRaw("Vertical") == 0 && PlayerAttack.attackInstance.isAttacking && ((PlayerAttack.attackInstance.playerMovement.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (PlayerAttack.attackInstance.playerMovement.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)))
        {
            PlayerAttack.attackInstance.anim.Play("BThrow");
            SetThrow();
            PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[2] = true;
        }
    }

    void SetThrow()
    {
        PlayerAttack.attackInstance.isAttacking = false;
        PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Animator>().SetBool("isInThrow", true);
        PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Animator>().SetBool("isGrabbed", false);
        PlayerAttack.attackInstance.currentlyGrabbedEnemy.GetComponent<Animator>().ResetTrigger("isGrabbed 0");
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
