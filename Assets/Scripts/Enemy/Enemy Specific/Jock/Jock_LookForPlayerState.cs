using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_LookForPlayerState : LookForPlayerState
{
    private Jock jock;

    public Jock_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, Jock jock) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(jock.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}