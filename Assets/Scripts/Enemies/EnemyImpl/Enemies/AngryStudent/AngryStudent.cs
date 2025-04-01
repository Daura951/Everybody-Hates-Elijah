using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryStudent : Entity
{
    public AS_IdleState idleState { get; private set; }
    public AS_MoveState moveState { get; private set; }
    public AS_PlayerDetectedState playerDetectedState { get; private set; }
    public AS_ChargeState chargeState { get; private set; }
    public AS_LookForPlayerState lookForPlayerState { get; private set; }

    public AS_MeleeState punchState { get; private set; }
    public AS_MeleeState kickState { get; private set; }

    public AS_StunState stunState { get; private set; }

    public AS_LaunchState launchState { get; private set; }

    public AS_GetupState getupState { get; private set; }

    public AS_DeadState launchDeadState { get; private set; }
    public AS_DeadState deadState { get; private set; }


    [SerializeField]
    private D_IdleState idleData;
    [SerializeField]
    private D_MoveState moveData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    public override void Start()
    {
        base.Start();
        moveState = new AS_MoveState(this, stateMachine, "move", moveData, this);
        idleState = new AS_IdleState(this, stateMachine, "idle", idleData, this);
        playerDetectedState = new AS_PlayerDetectedState(this, stateMachine, "detected", playerDetectedData, this);
        chargeState = new AS_ChargeState(this, stateMachine, "charge", chargeData, this);
        lookForPlayerState = new AS_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        punchState = new AS_MeleeState(this, stateMachine, "punch", this);
        kickState = new AS_MeleeState(this, stateMachine, "kick", this);
        stunState = new AS_StunState(this, stateMachine, "stun", this);
        launchState = new AS_LaunchState(this, stateMachine, "launch", this);
        deadState = new AS_DeadState(this, stateMachine, "dead", this);
        launchDeadState = new AS_DeadState(this, stateMachine, "launch dead", this);
        getupState = new AS_GetupState(this, stateMachine, "getup", this);
        stateMachine.Initalize(moveState);
    }

    public override void OnCutsceneBeginAndEnd()
    {
        base.OnCutsceneBeginAndEnd();
        stateMachine.ChangeState(idleState);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if (stateMachine.currentState != stunState && stateMachine.currentState != launchState)
        {
            stateMachine.ChangeState(stunState);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDead)
        {
            if(stateMachine.currentState == launchState)
            {
                stateMachine.ChangeState(launchDeadState);
            }
            else stateMachine.ChangeState(deadState);
        }
    }
}
