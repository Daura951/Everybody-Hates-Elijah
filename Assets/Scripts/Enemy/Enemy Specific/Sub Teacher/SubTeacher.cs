using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTeacher : Entity
{
    public SUB_IdleState idleState { get; private set; }
    public SUB_MoveState moveState { get; private set; }
    public SUB_PlayerDetectedState playerDetectedState { get; private set;}
    public SUB_RunAtState runAtState { get; private set; }
    public SUB_LookForPlayerState lookForPlayerState { get; private set; }

    public SUB_DeadState deadState;
    
    public SUB_SlashState slashState { get; private set; }

    public SUB_StunState stunState { get; private set; }

    public SUB_GrabState grabState { get; private set; }

    public SUB_PummelState pummelState { get; private set; }
    
    public SUB_BoomerangState boomerangState { get; private set; }

    public GameObject boomerang;

    public GameObject[] hitboxes;

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedData;
    [SerializeField] private D_RunAtState runAtStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerData;
    [SerializeField] private D_MeleeAttack slashStateData;
    [SerializeField] private D_RangedAttackState boomerangStateData;
    [SerializeField] private Transform slashPosition;
    [SerializeField] private Transform boomerangPosition;
    [SerializeField] private AnimatorOverrideController AOC;

    public Transform playerCheckTFT;

    public override void Start()
    {
        base.Start();
        if( 1 == Random.Range(1, 3))
        {
            isMain = false;
            Vocals.Replace();
            anim.runtimeAnimatorController = AOC;
        }
        moveState = new SUB_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new SUB_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new SUB_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        runAtState = new SUB_RunAtState(this, stateMachine, "runAt", runAtStateData, this);
        lookForPlayerState = new SUB_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        slashState = new SUB_SlashState(this, stateMachine, "punch", slashPosition, slashStateData, this);
        stunState = new SUB_StunState(this, stateMachine, "stun", this);
        deadState = new SUB_DeadState(this, stateMachine, "dead", this);
        grabState = new SUB_GrabState(this, stateMachine, "grabbed", this);
        pummelState = new SUB_PummelState(this, stateMachine, "pummel", this);
        boomerangState = new SUB_BoomerangState(this, stateMachine, "boomerang", boomerangPosition, boomerangStateData, this);
       
        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        if (!isInCutscene)
        {
            base.Update();

            if (health.GetHealth() <= 0)
            {
                stateMachine.ChangeState(deadState);
            }

        }

        else
        {
            stateMachine.ChangeState(idleState);
        }
    }

    public  override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void Damage()
    {
        base.Damage();
        if (!BB.isFrozen)
        {
            if (stateMachine.currentState != stunState && health.GetHealth() > 0)
            {
                stateMachine.ChangeState(stunState);
            }
            else if (health.GetHealth() <= 0)
            {
                stateMachine.ChangeState(deadState);
            }
            else if(checkPlayerInMinAgroRange())
            {
                stateMachine.ChangeState(boomerangState);
            }
        }
    }

    public override void GetGrabbed()
    {
        base.GetGrabbed();
        stateMachine.ChangeState(grabState);

    }

    public override void GetPummeled()
    {
        base.GetPummeled();
        stateMachine.ChangeState(pummelState);
        
    }

    public override void getThrown()
    {
        base.getThrown();
        stateMachine.ChangeState(stunState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" && stateMachine.currentState == stunState || stateMachine.currentState == deadState)
        {
            SetVelocity(0);

            if(health.GetHealth() > 0)
                stateMachine.ChangeState(idleState);
        }
    }

}
