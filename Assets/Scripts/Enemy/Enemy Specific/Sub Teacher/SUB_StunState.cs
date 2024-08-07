using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_StunState : StunState
{
    private SubTeacher subTeacher;

    public SUB_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName)
    {
        this.subTeacher = subTeacher;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        if (subTeacher.rb.velocity.y == 0)
        {
            subTeacher.SetVelocity(0);
        }

        if(isStunOver)
        {
            if(performCloseRangeAttack)
            {
                stateMachine.ChangeState(subTeacher.slashState);
            }
            else if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(subTeacher.runAtState);
            }
            else
            {
                subTeacher.lookForPlayerState.SetTurnImmedietly(true);
                stateMachine.ChangeState(subTeacher.lookForPlayerState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
