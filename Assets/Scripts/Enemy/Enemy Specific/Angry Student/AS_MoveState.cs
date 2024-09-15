using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_MoveState : MoveState
{
    private AngryStudent angryStudent;
    public AS_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.angryStudent = angryStudent;
    }

    public override void Enter()
    {
        foreach (GameObject hb in angryStudent.hitboxes)
        {
            hb.SetActive(false);
        }
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            angryStudent.idleState.setFlipAfterIdle(true);
            stateMachine.ChangeState(angryStudent.idleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
