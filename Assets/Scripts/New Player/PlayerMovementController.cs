using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerComponent
{
    [SerializeField]
    private Player player;
    private InputManager input;
    private AnimatorController animController;
    private PlayerAttackController attackController;
    public override void OnStart()
    {
        input = player.getInputManager();
        animController = player.getAnimController();
        attackController = player.GetAttackController();
        print("Hello PlayerMovementController!");
    }

    public override void OnUpdate()
    {
        if (!attackController.GetIsAttacking())
        {
            if (input.moveHorizontal != 0)
            {
                animController.Play(Animations.WALK, false, false);
            }
            else animController.Play(Animations.IDLE, false, false);

            transform.Translate(new Vector2(input.moveHorizontal / 100.0f, 0.0f));

            if (input.isJumping)
            {
                print("Jump");
            }
        }
    }
    public override void OnFixedUpdate()
    {
    }
}
