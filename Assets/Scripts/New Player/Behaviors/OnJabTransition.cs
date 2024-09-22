using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJabTransition : StateMachineBehaviour
{
    [SerializeField]
    private float crossFadeTime = 0.2f;

    public Animations animToGoToOnButtonPress;
    private InputManager manager;
    private PlayerAttackController attackController;
    private AnimatorController animController;

    private bool hasTransitioned = false;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        manager = animator.GetComponent<InputManager>();
        hasTransitioned = false;
        attackController = animator.GetComponent<PlayerAttackController>();
        animController = animator.GetComponent<AnimatorController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasTransitioned && manager.isNeutralPressed && manager.moveVertical ==0 && manager.moveHorizontal == 0)
        {
            animController.Play(animToGoToOnButtonPress, false, true, crossFadeTime);
            hasTransitioned = true;
        }

        else if(!hasTransitioned && manager.isNeutralPressed)
        {
            animController.Play(DetermineTilt(), false, true, crossFadeTime);
            hasTransitioned = true;
        }

        if (!hasTransitioned && stateInfo.normalizedTime >= 0.95f)
        {
            animController.Play(Animations.IDLE, false, true, crossFadeTime);
            animator.GetComponent<PlayerState>().isAttacking = false;
            hasTransitioned = true;
        }
    }

    public Animations DetermineTilt()
    {
        float moveY = manager.moveVertical;

        if (moveY > 0)
        {
            return Animations.UTILT;
        }

        else if (moveY < 0)
        {
            return Animations.DTILT;
        }

        else
        {
            return Animations.FTILT;
        }
    }
}