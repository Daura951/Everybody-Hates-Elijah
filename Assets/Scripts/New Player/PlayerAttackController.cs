using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;

    private bool isAttacking = false;
    public override void OnStart()
    {
        print("Hello PlayerAttackController!");
    }

    public override void OnUpdate()
    {
    }

    public void Jab()
    {
        if(!isAttacking)
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

    public void Configure(InputManager inputManager, AnimatorController animController)
    {
        this.inputManager = inputManager;
        this.animController = animController;

        inputManager.OnJabPressed += Jab;
    } 

}