using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAtState : State
{
    private D_RunAtState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isRunAtDone;
    protected bool performCloseRangeAttack;
    public RunAtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunAtState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.runAtSpeed);
        isRunAtDone = false;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime+stateData.runAtTime)
        {
            isRunAtDone = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        isPlayerInMinAgroRange = entity.checkPlayerInMinAgroRange();
        isDetectingLedge = entity.checkLedge();
        isDetectingWall = entity.CheckWall();
        performCloseRangeAttack = entity.checkPlayerInCloseRangeAttack();
    }
}