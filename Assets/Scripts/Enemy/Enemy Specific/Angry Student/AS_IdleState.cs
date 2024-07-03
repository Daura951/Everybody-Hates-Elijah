using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_IdleState : IdleState
{
    private AngryStudent angryStudent;
    public AS_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.angryStudent = angryStudent;
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
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(angryStudent.playerDetectedState);
            }
            else if (isIdleDone)
            {
                stateMachine.ChangeState(angryStudent.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}