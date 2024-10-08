using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNspecialTransition : StateMachineBehaviour
{
    private Animations animation;
    [SerializeField] private bool isLocked;
    [SerializeField] private float crossFadeTime = 0.2f;
    [SerializeField] private bool isAnimAttack;

    [HideInInspector] public bool cancel = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isAnimAttack)
        {
            animator.GetComponent<PlayerState>().isAttacking = true;
        }

        animator.GetComponent<MonoBehaviour>().StartCoroutine(Transition(animator, stateInfo));
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool didStickyHit = animator.GetComponent<AttackManager>().didStickyCollide;
        animation = didStickyHit ?  Animations.NSPECIAL_SUCESS : Animations.NSPECIAL_FAIL;
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
    }

    private IEnumerator Transition(Animator animator, AnimatorStateInfo stateInfo)
    {
        yield return new WaitForSeconds(stateInfo.length - crossFadeTime);

        if (cancel)
        {
            yield break; // If cancelled, exit coroutine
        }

        PlayerState playerstate = animator.GetComponent<PlayerState>();
        AnimatorController animatorController = animator.GetComponent<AnimatorController>();

        // Play Jab 1 Transition immediately
        animatorController.Play(animation, isLocked, true, crossFadeTime);
    }

}
