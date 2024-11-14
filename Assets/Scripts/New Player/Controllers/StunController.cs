using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;


    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;

    }


    public override void OnFixedUpdate()
    {

    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {


        SendDataToPlayerState();
    }

    private void SendDataToPlayerState()
    {

    }

}
