using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerstate;

    private bool isSmashCharging = false;

    [SerializeField]
    private float tiltBuffer = .1f;

    [SerializeField]
    private float specialBuffer = .1f;

    [SerializeField]
    private float tiltThreshold = 0.1f;
    
    [SerializeField]
    private float smashThreshold = 0.8f;

    public override void OnStart()
    {
        print("Hello PlayerAttackController!");
    }

    public override void OnUpdate()
    {
    }

    public void PerformNeutral()
    {
        Vector2 move = new Vector2(inputManager.moveHorizontal, inputManager.moveVertical);
        if (!playerstate.isAttacking)
        {
            playerstate.isAttacking = true;
            if (playerstate.isGrounded)
            {
                if(playerstate.isRunning)
                {
                    Dash();
                    return;
                }

                if (move.magnitude < tiltThreshold)
                {
                    Jab();
                    return;
                }
                Tilt(move.y);
                return;
            }
            Ariel(move);
        }
    }


    private void Jab()
    {
        animController.Play(Animations.JAB_1, false, false);
    }

    private void Dash()
    {
        animController.Play(Animations.DASH, false, false);
    }

    private void Tilt(float moveY)
    {
        if (moveY > tiltBuffer)
        {
            animController.Play(Animations.UTILT, false, false);
        }
        else if (moveY < -tiltBuffer)
        {
            animController.Play(Animations.DTILT, false, false);
        }
        else
        {
            animController.Play(Animations.FTILT, false, false);
        }

    }

    private void Ariel(Vector2 move)
    {
        if (move.y> tiltBuffer)
        {
            animController.Play(Animations.UAIR, false, false);
        }
        else if (move.y < -tiltBuffer)
        {
            animController.Play(Animations.DAIR, false, false);
        }
        else if((move.x > tiltBuffer && playerstate.isRight) || (move.x < -tiltBuffer && !playerstate.isRight))
        {
            animController.Play(Animations.FAIR, false, false);
        }
        else if ((move.x < -tiltBuffer && playerstate.isRight) || (move.x > tiltBuffer && !playerstate.isRight))
        {
            animController.Play(Animations.BAIR, false, false);
        }
        else
        {
            animController.Play(Animations.NAIR, false, false);
        }
    }
    private void StrongHold()
    {
        if(!playerstate.isGrounded)
        {
            Ariel(new Vector2(inputManager.moveHorizontal, inputManager.moveVertical));
        }

        else if(!playerstate.isAttacking &&!animController.GetCurrentAnimation().ToString().Contains("HIT"))
        {
            playerstate.isAttacking = true;
            isSmashCharging = true;
            float moveY = inputManager.moveVertical;

            if (moveY > 0+ specialBuffer)
            {
                animController.Play(Animations.USTRONG_CHARGE, false, false);
            }
            else if (moveY < 0- specialBuffer)
            {
                animController.Play(Animations.DSTRONG_CHARGE, false, false);
            }
            else
            {
                animController.Play(Animations.FSTRONG_CHARGE, false, false);
            }
        }
    }

    private void StrongRelease()
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

    private void Special()
    {
        if (!playerstate.isAttacking)
        {
            playerstate.isAttacking = true;
            Vector2 move = new Vector2(inputManager.moveHorizontal, inputManager.moveVertical);

            if (move.y > specialBuffer)
            {
                animController.Play(Animations.USPECIAL, false, false);
            }
            else if (move.y < -specialBuffer)
            {
                animController.Play(Animations.DSPECIAL, false, false);
            }
            else if(move.x != 0)
            {
               animController.Play(Animations.SSPECIAL, false, false);
            }
            else if(!playerstate.isBBReady)
            {
                animController.Play(Animations.NSPECIAL_STARTUP, false, false);
            }
            else
            {
                playerstate.isAttacking = false; //TODO: Make sure to get rid of this for when BB anim is in
            }
        }
    }

    public override void OnFixedUpdate()
    {
    }


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerstate = playerstate;

        inputManager.OnNeutralPressed += PerformNeutral;
        inputManager.OnStrongHold += StrongHold;
        inputManager.OnStrongRelease += StrongRelease;
        inputManager.OnSpecialPressed += Special;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(animController.GetCurrentAnimation().ToString().Contains("AIR"))
        {
            animController.Play(Animations.IDLE, false, false);
        }
    }

}