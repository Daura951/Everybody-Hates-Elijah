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
        Animator.StringToHash("Double Jump"),
        Animator.StringToHash("Falling"),
        Animator.StringToHash("Jab 1"),
        Animator.StringToHash("Jab 1 Transition"),
        Animator.StringToHash("Jab 2"),
        Animator.StringToHash("Jab 2 Transition"),
        Animator.StringToHash("Jab 3"),
        Animator.StringToHash("Jab 3 Transition"),
        Animator.StringToHash("DTilt"),
        Animator.StringToHash("UTilt"),
        Animator.StringToHash("FTilt"),
        Animator.StringToHash("NAir"),
        Animator.StringToHash("UAir"),
        Animator.StringToHash("FAir"),
        Animator.StringToHash("BAir"),
        Animator.StringToHash("DAir"),
        Animator.StringToHash("DStrong Charge"),
        Animator.StringToHash("DStrong Hit"),
        Animator.StringToHash("UStrong Charge"),
        Animator.StringToHash("UStrong Hit"),
        Animator.StringToHash("FStrong Charge"),
        Animator.StringToHash("FStrong Hit"),
        Animator.StringToHash("DSpecial"),
        Animator.StringToHash("SSpecial"),
        Animator.StringToHash("USpecial"),
        Animator.StringToHash("NSpecial Elijah Strartup"),
        Animator.StringToHash("NSpecial Elijah Fail"),
        Animator.StringToHash("NSpecial Elijah Success")

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
       // print("Going from: " + currentAnimation.ToString() + " to: " + animation.ToString());
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
        //print(currentAnimation.ToString() + " "+(int)currentAnimation);
        anim.CrossFade(animations[(int)currentAnimation], crossFadeTime);
    }

}


public enum Animations
{
    IDLE,
    WALK,
    RUN,
    SINGLE_JUMP,
    DOUBLE_JUMP,
    FALLING,
    JAB_1,
    JAB_1_TRANSITION,
    JAB_2,
    JAB_2_TRANSITION,
    JAB_3,
    JAB_3_TRANSITION,
    DTILT,
    UTILT,
    FTILT,
    NAIR,
    UAIR,
    FAIR,
    BAIR,
    DAIR,
    DSTRONG_CHARGE,
    DSTRONG_HIT,
    USTRONG_CHARGE,
    USTRONG_HIT,
    FSTRONG_CHARGE,
    FSTRONG_HIT,
    DSPECIAL,
    SSPECIAL,
    USPECIAL,
    NSPECIAL_STARTUP,
    NSPECIAL_FAIL,
    NSPECIAL_SUCESS,
    NONE
}
