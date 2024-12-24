using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunController : PlayerComponent
{

    public float StunTimer = 0;

    public override void Configure(InputManager inputManager, AnimatorController animController, PlayerState playerstate)
    {
        base.Configure(inputManager, animController, playerstate);

    }

    public override void OnStart()
    {

    }

    public override void OnFixedUpdate()
    {

    }


    public override void OnUpdate()
    {
        if(playerState.isStunned)
        {
            StunTimer -= StunTimer <= 0 ? 0 : Time.deltaTime;
            playerState.isStunned = StunTimer <= 0 ? false : true;

            if(!playerState.isStunned)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedGO = collision.gameObject;
        if(collidedGO.gameObject.tag == "EHitbox")
        {
            AttackDetails hit = collidedGO.GetComponentInParent<EnemyAttackManager>().findByHitbox(collidedGO.name);
            ApplayStun(hit.StunTime);
            ApplyStunRandomizer();
        }
    }


    private void ApplayStun(float Stunner)
    {
        StunTimer = Stunner;
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
