using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJabTransition : StateMachineBehaviour
{
    public Animations animToGoToOnButtonPress;  // Jab 2
    public Animations animToGoToOnEnd;          // Idle

    [SerializeField]
    private float crossFadeTime = 0.2f;

    private InputManager manager;
    private bool hasTransitioned = false; // To prevent multiple transitions

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        manager = animator.GetComponent<InputManager>();
        hasTransitioned = false; // Reset when entering the state
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasTransitioned && manager.isNeturalPressed)
        {
            animator.GetComponent<AnimatorController>().Play(animToGoToOnButtonPress, false, true, crossFadeTime);
            hasTransitioned = true;
          //  Debug.Log("TRANSITION TO " + animToGoToOnButtonPress.ToString());
            return;
        }

        if (!hasTransitioned && stateInfo.normalizedTime >= 0.95f)
        {
            animator.GetComponent<AnimatorController>().Play(animToGoToOnEnd, false, false, crossFadeTime);
          //  Debug.Log("TRANSITION TO " + animToGoToOnEnd.ToString());
            animator.GetComponent<PlayerAttackController>().SetIsAttacking(false);
            hasTransitioned = true;
        }
    }
}