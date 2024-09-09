using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_GrabState : GrabState
{
    private LeverFlipper leverFlipper;

    public LF_GrabState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isFree)
        {
            stateMachine.ChangeState(leverFlipper.awayFromLeverIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}