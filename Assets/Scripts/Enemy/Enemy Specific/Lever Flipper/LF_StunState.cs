using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_StunState : StunState
{
    private LeverFlipper leverFlipper;

    public LF_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName)
    {
        this.leverFlipper = leverFlipper;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        leverFlipper.isLeverPulled = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (leverFlipper.rb.velocity.y == 0)
        {
            leverFlipper.SetVelocity(0);
        }

        if(isStunOver)
        {
            stateMachine.ChangeState(leverFlipper.awayFromLeverIdleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
