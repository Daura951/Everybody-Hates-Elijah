using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationExit : StateMachineBehaviour
{
    [SerializeField] private Animations animation;
    [SerializeField] private bool isLocked;
    [SerializeField] private float crossFadeTime = 0.2f;
    [SerializeField] private bool isAnimAttack;
    [SerializeField] private bool isJab, isStrong, isLedgeMove, LedgeAnim;

     [HideInInspector] public bool cancel = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cancel = false;

        if (isAnimAttack)
        {
            animator.GetComponent<PlayerState>().isAttacking = true;
        }

        animator.GetComponent<MonoBehaviour>().StartCoroutine(Transition(animator, stateInfo));
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
        LedgeGrabController LGC = animator.GetComponent<LedgeGrabController>();

        if (!isJab && !isStrong)
        {
            playerstate.isAttacking = false;
        }

        if(LedgeAnim)
        {
            animatorController.SetIsLocked(false);
            playerstate.isLedgeGrab = false;
            LGC.LedgeRotate();

            if (isLedgeMove)
            {
                LGC.LedgeMove();

            }
        }



        // Play Jab 1 Transition immediately
        animatorController.Play(animation, isLocked, true, crossFadeTime);
    }
}
