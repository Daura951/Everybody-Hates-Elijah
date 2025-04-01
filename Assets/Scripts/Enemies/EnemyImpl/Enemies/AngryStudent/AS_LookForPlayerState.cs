using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_LookForPlayerState : LookForPlayerState
{
    private AngryStudent angryStudent;

    public AS_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
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
        if(isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(angryStudent.playerDetectedState);
        }
        else if(isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(angryStudent.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }
}
