using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunController : PlayerComponent
{
    private InputManager inputManager;
    private AnimatorController animController;
    private PlayerState playerState;

    public float StunTimer = 0;
    public bool IsStunned = false ;

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        this.inputManager = inputManager;
        this.animController = animController;
        this.playerState = playerstate;

    }

    public override void OnStart()
    {

    }

    public override void OnFixedUpdate()
    {

    }


    public override void OnUpdate()
    {
        if(IsStunned)
        {
            StunTimer -= StunTimer <= 0 ? 0 : Time.deltaTime;
            IsStunned = StunTimer <= 0 ? false : true;

            if(!IsStunned)
                EventManager.TriggerOnStunEnd();
        }

        SendDataToPlayerState();
    }

    private void SendDataToPlayerState()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject collidedGO = collision.gameObject;
        if (collidedGO.GetComponent<Stun_Info>() != null)
        {
            print("STUN HAZARD!");
            ApplayStun(collidedGO.GetComponent<Stun_Info>().GetDAKTInfo()[3]);
            ApplyStunRandomizer();
        }
    }


    private void ApplayStun(float Stunner)
    {
        StunTimer = Stunner;
        IsStunned = true;
    }

    private void ApplyStunRandomizer()
    {
        int randStun = UnityEngine.Random.Range(1, 4);
        if(randStun == 1)
        animController.Play(Animations.Stun1, false, true);
        else if (randStun == 2)
        animController.Play(Animations.Stun2, false, true);
        else
        animController.Play(Animations.Stun3, false, true);
    }



}
