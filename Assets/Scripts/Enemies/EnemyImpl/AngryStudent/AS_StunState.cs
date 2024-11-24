using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_StunState : StunState
{
    private AngryStudent angryStudent;

    public AS_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
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
        if(entity.currentHit.StunTime != stunTime)
        {
            stunTime += entity.currentHit.StunTime;
        }

        if(isStunTimeOver)
        {
            if(performCloseRangeAction)
            {
                int attackChoice = Random.Range(1, 3);
                stateMachine.ChangeState((attackChoice == 1 ? angryStudent.punchState : angryStudent.kickState));
            }
            else if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(angryStudent.chargeState);
            }
            else
            {
                angryStudent.lookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(angryStudent.lookForPlayerState);
            }
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
