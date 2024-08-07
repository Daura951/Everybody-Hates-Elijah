using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_PlayerDetectedState : PlayerDetectedState
{
    private SubTeacher subTeacher;
    public SUB_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, stateData)
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
            if (performCloseRangeAttack)
            {
                stateMachine.ChangeState(subTeacher.slashState);
            }

            else if(performLongRangeAttack)
            {
                stateMachine.ChangeState(subTeacher.boomerangState);
            }

            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(subTeacher.lookForPlayerState);
            }

        }


        else
        {
            stateMachine.ChangeState(subTeacher.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
