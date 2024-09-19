using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private readonly static int[] animations =
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Walk"),
        Animator.StringToHash("Run"),
        Animator.StringToHash("Single Jump"),
        Animator.StringToHash("Falling"),
        Animator.StringToHash("Jab 1"),
        Animator.StringToHash("Jab 1 Transition"),
        Animator.StringToHash("Jab 2"),
        Animator.StringToHash("Jab 2 Transition"),
        Animator.StringToHash("Jab 3"),
        Animator.StringToHash("Jab 3 Transition")
    };

    [SerializeField]
    private Animator anim;

    private Animations currentAnimation = Animations.IDLE;
    private bool isLocked = false;


    public Animations GetCurrentAnimation()
    {
        return currentAnimation;
    }

    public void SetIsLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }

    public void Play(Animations animation, bool lockAnimator, bool bypassLock, float crossFadeTime = 0.2f)
    {
        //print("Going from: " + currentAnimation.ToString() + " to: " + animation.ToString());
        if(animation==Animations.NONE)
        {
            anim.CrossFade(animations[(int)Animations.IDLE], crossFadeTime);
            return;
        }

        if(isLocked && !bypassLock)
        {
            return;
        }

        isLocked = lockAnimator;

        if(bypassLock)
        {
            foreach(var item in anim.GetBehaviours<OnAnimationExit>())
            {
                item.cancel = true;
            }
        }

        if(currentAnimation==animation)
        {
            return;
        }

        currentAnimation = animation;

        anim.CrossFade(animations[(int)currentAnimation], crossFadeTime);
    }

}


public enum Animations
{
    IDLE,
    WALK,
    RUN,
    SINGLE_JUMP,
    FALLING,
    JAB_1,
    JAB_1_TRANSITION,
    JAB_2,
    JAB_2_TRANSITION,
    JAB_3,
    JAB_3_TRANSITION,
    NONE
}
