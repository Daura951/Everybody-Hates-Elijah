using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_LookForPlayerState : LookForPlayerState
{
    private SubTeacher subTeacher;

    public SUB_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, stateData)
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
        if(isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(subTeacher.playerDetectedState);
        }
        else if(isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(subTeacher.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}