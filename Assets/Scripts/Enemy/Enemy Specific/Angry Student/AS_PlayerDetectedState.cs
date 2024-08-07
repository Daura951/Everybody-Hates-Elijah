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
            if (performCloseRangeAttack)
            {
             if (1 == Random.Range(1, 3))
                stateMachine.ChangeState(angryStudent.punchState);
             else
                stateMachine.ChangeState(angryStudent.kickState);
            }

            else if (performLongRangeAttack)
            {
                stateMachine.ChangeState(angryStudent.runAtState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(angryStudent.lookForPlayerState);
            }
        }
        else
        {
            stateMachine.ChangeState(angryStudent.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
