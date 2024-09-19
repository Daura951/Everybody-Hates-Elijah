using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationExit : StateMachineBehaviour
{
    [SerializeField] private Animations animation;
    [SerializeField] private bool isLocked;
    [SerializeField] private float crossFadeTime = 0.2f;
    [SerializeField] private bool isAnimAttack;
    [SerializeField] private bool isJab;

    [HideInInspector] public bool cancel = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cancel = false;

        if (isAnimAttack)
        {
            animator.GetComponent<PlayerAttackController>().SetIsAttacking(true);
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

        PlayerAttackController attackController = animator.GetComponent<PlayerAttackController>();
        AnimatorController animatorController = animator.GetComponent<AnimatorController>();


        if (attackController.GetIsAttacking() && !isJab)
        {
            attackController.SetIsAttacking(false);
        }

        // Play Jab 1 Transition immediately
        animatorController.Play(animation, isLocked, false, crossFadeTime);
    }
}
