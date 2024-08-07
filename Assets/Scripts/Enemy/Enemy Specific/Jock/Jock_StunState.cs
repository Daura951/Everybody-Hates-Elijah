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

        if(!isStunOver)
        {

        }


        if (isStunOver)
        {
            if (performCloseRangeAttack)
            {
                    stateMachine.ChangeState(jock.punchState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(jock.runAtState);
            }
            else
            {
                jock.lookForPlayerState.SetTurnImmedietly(true);
                stateMachine.ChangeState(jock.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
