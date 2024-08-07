using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_IdleState : IdleState
{
    private SubTeacher subTeacher;
    public SUB_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.subTeacher = subTeacher;
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
                stateMachine.ChangeState(subTeacher.playerDetectedState);
            }
            else if (isIdleDone)
            {
                stateMachine.ChangeState(subTeacher.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}