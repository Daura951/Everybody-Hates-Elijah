using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_RunAtState : RunAtState
{
    private Jock jock;
    public Jock_RunAtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunAtState stateData, Jock jock) : base(entity, stateMachine, animBoolName, stateData)
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

        if (performCloseRangeAttack)
        {
                stateMachine.ChangeState(jock.punchState);
        }

        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(jock.lookForPlayerState);
        }

        else if (isRunAtDone)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(jock.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(jock.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
