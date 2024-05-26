using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;
    protected bool isDetectingWall, isDetectingLedge;
    protected bool isPlayerInMinAgroRange;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!entity.BB.GetIsInBladeBound())
        {
            entity.SetVelocity(stateData.moveSpeed);
        }
        else
        {
            entity.SetVelocity(stateData.moveSpeed / 2);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.checkLedge();
        isDetectingWall = entity.CheckWall();
        isPlayerInMinAgroRange = entity.checkPlayerInMinAgroRange();
    }
}
