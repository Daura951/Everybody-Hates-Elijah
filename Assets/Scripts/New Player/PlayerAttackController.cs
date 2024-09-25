using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerstate;

    private bool isSmashCharging = false;

    public override void OnStart()
    {
        print("Hello PlayerAttackController!");
    }

    public override void OnUpdate()
    {
    }

    public void Jab()
    {
        if(!playerstate.isAttacking)
        {
            animController.Play(Animations.JAB_1, false, false);
            playerstate.isAttacking = true;
        }
    }

    public void Tilt()
    {
        if (!playerstate.isAttacking)
        {
            playerstate.isAttacking = true;
            float moveY = inputManager.moveVertical;

            if (moveY > 0)
            {
                animController.Play(Animations.UTILT, false, false);
            }
            else if (moveY < 0)
            {
                animController.Play(Animations.DTILT, false, false);
            }
            else
            {
                animController.Play(Animations.FTILT, false, false);
            }
        }
    }

    public void StrongHold()
    {
        if(!playerstate.isAttacking &&!animController.GetCurrentAnimation().ToString().Contains("HIT"))
        {
            playerstate.isAttacking = true;
            isSmashCharging = true;
            float moveY = inputManager.moveVertical;

            if (moveY > 0)
            {
                animController.Play(Animations.USTRONG_CHARGE, false, false);
            }
            else if (moveY < 0)
            {
                animController.Play(Animations.DSTRONG_CHARGE, false, false);
            }
            else
            {
                animController.Play(Animations.FSTRONG_CHARGE, false, false);
            }
        }
    }

    public void StrongRelease()
    {
        if (isSmashCharging &&(animController.GetCurrentAnimation().ToString().Contains("CHARGE")))
        {
            print("Strong release!");

            if (animController.GetCurrentAnimation() == Animations.USTRONG_CHARGE)
            {
                animController.Play(Animations.USTRONG_HIT, false, false);
            }
            else if (animController.GetCurrentAnimation() == Animations.DSTRONG_CHARGE)
            {
                animController.Play(Animations.DSTRONG_HIT, false, false);
            }
            else if(animController.GetCurrentAnimation() == Animations.FSTRONG_CHARGE)
            {
                animController.Play(Animations.FSTRONG_HIT, false, false);
            }
        }
            isSmashCharging = false;
    }


    public override void OnFixedUpdate()
    {
    }


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerstate = playerstate;

        inputManager.OnJabPressed += Jab;
        inputManager.OnTiltPressed += Tilt;
        inputManager.OnStrongHold += StrongHold;
        inputManager.OnStrongRelease += StrongRelease;
    }

}