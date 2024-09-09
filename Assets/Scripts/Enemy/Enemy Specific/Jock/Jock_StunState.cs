using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_StunState : StunState
{
    private Jock jock;

    public Jock_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Jock jock) : base(entity, stateMachine, animBoolName)
    {
        this.jock = jock;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        if (jock.rb.velocity.y == 0)
        {
            jock.SetVelocity(0);
        }

        if (isStunOver)
        {
            if (performCloseRangeAttack)
            {
                if (1 == Random.Range(1, 3))
                    stateMachine.ChangeState(jock.punchState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(jock.playerDetectedState);
            }
            else
            {
                jock.lookForPlayerState.SetTurnImmedietly(true);
                stateMachine.ChangeState(jock.lookForPlayerState);
            }
        }

        else if (entity.Yspeed > 8f)
        {
            stateMachine.ChangeState(jock.launchState);
        }
        else if (entity.Yspeed < -2.6f && !entity.CheckGround())
        {
            stateMachine.ChangeState(jock.spinState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
