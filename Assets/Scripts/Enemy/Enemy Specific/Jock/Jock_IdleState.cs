using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_IdleState : IdleState
{
    private Jock jock;
    public Jock_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Jock jock) : base(entity, stateMachine, animBoolName, stateData)
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

            if (isPlayerInMinAgroRange && !entity.idleStun)
            {
                stateMachine.ChangeState(jock.playerDetectedState);
            }
            else if (isIdleDone)
            {
                stateMachine.ChangeState(jock.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}