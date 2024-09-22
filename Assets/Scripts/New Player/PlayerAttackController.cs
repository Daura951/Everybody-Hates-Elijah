using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerstate;
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

    public void Smash()
    {

    }


    public override void OnFixedUpdate()
    {
    }


    public void Configure(InputManager inputManager, AnimatorController animController)
    {
        this.inputManager = inputManager;
        this.animController = animController;

        inputManager.OnJabPressed += Jab;
        inputManager.OnTiltPressed += Tilt;
        inputManager.OnSmashPressed += Smash;
    }

}