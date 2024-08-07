using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_PlayerDetectedState : PlayerDetectedState
{
    private Jock jock;
    public Jock_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Jock jock) : base(entity, stateMachine, animBoolName, stateData)
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

        if (!entity.BB.isFrozen)
        {
            if (performCloseRangeAttack)
            {
                    stateMachine.ChangeState(jock.punchState);
            }

            else if (performLongRangeAttack)
            {
                stateMachine.ChangeState(jock.runAtState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(jock.lookForPlayerState);
            }
        }
        else
        {
            stateMachine.ChangeState(jock.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
