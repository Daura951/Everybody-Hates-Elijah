using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_PlayerDetectedState : PlayerDetectedState
{

    private AngryStudent angryStudent;
    public AS_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.angryStudent = angryStudent;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performShortRangeAction)
        {
            int attackChoice = Random.Range(1, 3);
            stateMachine.ChangeState((attackChoice == 1 ? angryStudent.punchState : angryStudent.kickState));
        }

        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(angryStudent.chargeState);
        }
        else if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(angryStudent.lookForPlayerState);
        }
        else if(!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(angryStudent.moveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
