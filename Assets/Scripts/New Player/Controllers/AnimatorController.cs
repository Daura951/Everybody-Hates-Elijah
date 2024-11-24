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
        Animator.StringToHash("FSpecial"),
        Animator.StringToHash("USpecial"),
        Animator.StringToHash("NSpecial Elijah Strartup"),
        Animator.StringToHash("NSpecial Elijah Fail"),
        Animator.StringToHash("NSpecial Elijah Success"),
        Animator.StringToHash("Dash"),
        Animator.StringToHash("BladeboundActivation"),
        Animator.StringToHash("Ledge grab"),
        Animator.StringToHash("Ledge idle"),
        Animator.StringToHash("Ledge attack"),
        Animator.StringToHash("Ledge pull"),
        Animator.StringToHash("Stun1"),
        Animator.StringToHash("Stun2"),
        Animator.StringToHash("Stun3")
    };

    [SerializeField]
    private Animator anim;

    private Animations currentAnimation = Animations.IDLE;
    private bool isLocked = false;


    private void Awake()
    {
        EventManager.bladebound.AddListener(ConfigureBladeBound);
        EventManager.bladeboundEnd.AddListener(ConfigureAfterBladeBound);
        EventManager.onCutesceneEnter.AddListener(ConfigureForCutscene);
    }


    public Animations GetCurrentAnimation()
    {
        return currentAnimation;
    }

    public void SetIsLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }

    /// <summary>
    /// Plays the animation if able to
    /// </summary>
    /// <param name="animation">Animation to play</param>
    /// <param name="lockAnimator">Should the animator be locked</param>
    /// <param name="bypassLock">Should the animation bypass the lock</param>
    /// <param name="crossFadeTime">Time for crossfade</param>
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
        //print(currentAnimation.ToString() + " "+(int)currentAnimation);
        anim.CrossFade(animations[(int)currentAnimation], crossFadeTime);
    }

    public void ConfigureBladeBound()
    {
        anim.speed = 2.0f;
    }
    public void ConfigureAfterBladeBound()
    {
        anim.speed = 1.0f;
    }

    public void ConfigureForCutscene()
    {
        Play(Animations.IDLE, false, false);
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
    DASH,
    BLADEBOUND_ACTIVATION,
    LEDGE_GRAB,
    LEDGE_IDLE,
    LEDGE_ATTACK,
    LEDGE_PULL,
    Stun1,
    Stun2,
    Stun3,
    NONE
}
