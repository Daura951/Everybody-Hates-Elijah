using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSuccess : StateMachineBehaviour
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
        if (PR.Attack.isAttacking && Input.GetAxisRaw("Vertical")==0 && Input.GetAxisRaw("Horizontal")==0 &&PR.anim.GetBool("hasGrabbedEnemy") == true && !PR.Move.isInAir && !PR.Attack.isSpecial)
        {
            PR.anim.Play("Pummel");
            PR.Attack.isAttacking = false;

        }

        if(Input.GetAxisRaw("Vertical") > 0 && PR.Attack.isAttacking && Input.GetAxisRaw("Horizontal") == 0)
        {
            PR.anim.Play("UThrow");
            PR.Attack.isAttacking = false;
            SetThrow();
            //PR.Attack.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[0] = true;
        }

        if(Input.GetAxisRaw("Vertical") < 0 && PR.Attack.isAttacking && Input.GetAxisRaw("Horizontal") == 0 )
        {
            PR.anim.Play("DThrow");
            SetThrow();
            //PR.Attack.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[3] = true;
        }

        if(Input.GetAxisRaw("Vertical") == 0 && PR.Attack.isAttacking && ((PR.Move.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") > 0) || (PR.Move.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") < 0)))
        {
            PR.anim.Play("FThrow");
            SetThrow();
            //PR.Attack.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[1] = true;
        }

        if(Input.GetAxisRaw("Vertical") == 0 && PR.Attack.isAttacking && ((PR.Move.transform.rotation.y == 0 && Input.GetAxisRaw("Horizontal") < 0) || (PR.Move.transform.rotation.y < 0 && Input.GetAxisRaw("Horizontal") > 0)))
        {
            PR.anim.Play("BThrow");
            SetThrow();
            //PR.Attack.currentlyGrabbedEnemy.GetComponent<Enemy_Target>().whichThrow[2] = true;
        }
    }

    void SetThrow()
    {
        PR.Attack.isAttacking = false;
        //PR.Attack.currentlyGrabbedEnemy.GetComponent<Animator>().SetBool("isGrabbed", false);
        //PR.Attack.currentlyGrabbedEnemy.GetComponent<Animator>().ResetTrigger("isGrabbed 0");
        //PR.Attack.currentlyGrabbedEntity.getThrown();
        PR.Attack.Throw();
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
