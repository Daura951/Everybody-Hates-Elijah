using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDetectedState : State
{
    private D_PlayerDetected stateData;
    protected bool isPlayerInMinAgroRange, isPlayerInMaxAgroRange;
    protected bool performLongRangeAction, performShortRangeAction;
    protected bool isDetectingLedge;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        performLongRangeAction = false;
        performShortRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= startTime + stateData.longRangeWaitTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        performShortRangeAction = entity.CheckPlayerInCloseRangeAction();
        isDetectingLedge = entity.CheckLedge();
    }
}
