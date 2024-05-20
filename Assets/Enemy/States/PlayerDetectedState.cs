using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{

    private D_PlayerDetected stateData;
    protected bool isPlayerInMinAgroRange, isPlayerInMaxAgroRange;
    protected bool performLongRangeAttack, performCloseRangeAttack;
    protected bool isGrabbed;
    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0);
        performLongRangeAttack = false;
        isGrabbed = entity.isGrabbed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > startTime + stateData.longRangeActionTime)
        {
            performLongRangeAttack = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.checkPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.checkPlayerInMaxAgroRange();
        performCloseRangeAttack = entity.checkPlayerInCloseRangeAttack();
    }
}