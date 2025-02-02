using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : PlayerComponent
{

    public override void OnFixedUpdate()
    {

    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {

    }
    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);
        inputManager.OnGrabPressed += PerformGrab;
        
    }

    private void PerformGrab()
    {
        playerState.isAttacking = true;
        animController.Play(Animations.GRAB_START, false, false);
    }


}
