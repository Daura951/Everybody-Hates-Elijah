using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_RunAtState : RunAtState
{
    private SubTeacher subTeacher;
    public SUB_RunAtState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunAtState stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, stateData)
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

        if(performCloseRangeAttack)
        {
            stateMachine.ChangeState(subTeacher.slashState);
        }

        else if(!isDetectingLedge || isDetectingWall)
        {
           stateMachine.ChangeState(subTeacher.lookForPlayerState);
        }

        else if(isRunAtDone)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(subTeacher.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(subTeacher.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}