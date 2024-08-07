using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_MoveState : MoveState
{
    private SubTeacher subTeacher;
    public SUB_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            subTeacher.idleState.setFlipAfterIdle(true);
            stateMachine.ChangeState(subTeacher.idleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
