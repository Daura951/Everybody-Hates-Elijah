using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_MoveState : MoveState
{
    private Jock jock;
    public Jock_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Jock jock) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.jock = jock;
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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(jock.playerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            jock.idleState.setFlipAfterIdle(true);
            stateMachine.ChangeState(jock.idleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
