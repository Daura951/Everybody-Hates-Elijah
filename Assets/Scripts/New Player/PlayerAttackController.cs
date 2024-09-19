using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    [SerializeField]
    private Player player;
    private InputManager input;
    private AnimatorController animController;

    private bool isAttacking = false;
    public override void OnStart()
    {
        input = player.getInputManager();
        animController = player.getAnimController();
        print("Hello PlayerAttackController!");
    }

    public override void OnUpdate()
    {
        if(input.isNeturalPressed && !isAttacking)
        {
            animController.Play(Animations.JAB_1, false, false);
            isAttacking = true;
        }
    }
    public override void OnFixedUpdate()
    {
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

}