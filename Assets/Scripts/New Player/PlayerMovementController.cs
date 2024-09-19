using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerAttackController attackController;
    public override void OnStart()
    {
        print("Hello PlayerMovementController!");
    }

    public override void OnUpdate()
    {
        if (!attackController.GetIsAttacking())
        {

            if (inputManager.isJumping)
            {
                print("Jump");
            }
        }
    }
    public override void OnFixedUpdate()
    {
    }


    public void MovePlayer(float moveX)
    {
        if (!attackController.GetIsAttacking())
        {
            if (moveX != 0)
            {
                animController.Play(Animations.WALK, false, false);
            }
            else animController.Play(Animations.IDLE, false, false);

            transform.Translate(new Vector2(moveX / 100.0f, 0.0f));
        }
    }

    public void Configure(InputManager inputManager, AnimatorController animController, PlayerAttackController attackController)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.attackController = attackController;


        inputManager.OnMove+=MovePlayer;
    }
}
