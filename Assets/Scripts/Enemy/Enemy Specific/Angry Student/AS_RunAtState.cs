using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_RunAtState : RunAtState
{
    private AngryStudent angryStudent;
    public AS_RunAtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunAtState stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
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

        if(performCloseRangeAttack)
        {
            stateMachine.ChangeState(angryStudent.punchState);
        }

        else if(!isDetectingLedge || isDetectingWall)
        {
           stateMachine.ChangeState(angryStudent.lookForPlayerState);
        }

        else if(isRunAtDone)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(angryStudent.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(angryStudent.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}