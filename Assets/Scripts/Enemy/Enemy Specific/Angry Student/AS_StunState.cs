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

        if (angryStudent.rb.velocity.y == 0)
        {
            angryStudent.SetVelocity(0);
        }

        if (isStunOver)
        {
            if (performCloseRangeAttack)
            {
                if (1 == Random.Range(1, 3))
                    stateMachine.ChangeState(angryStudent.punchState);
                else
                    stateMachine.ChangeState(angryStudent.kickState);
            }
            else if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(angryStudent.runAtState);
            }
            else
            {
                angryStudent.lookForPlayerState.SetTurnImmedietly(true);
                stateMachine.ChangeState(angryStudent.lookForPlayerState);
            }
        }

        else if (entity.Yspeed > 6f)
        {
            stateMachine.ChangeState(angryStudent.launchState);
        }
        else if (entity.Yspeed < -.6f && !entity.CheckGround())
        {
            stateMachine.ChangeState(angryStudent.spinState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
