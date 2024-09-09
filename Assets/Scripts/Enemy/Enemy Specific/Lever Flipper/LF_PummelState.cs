using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_PummelState : PummelState
{
    private LeverFlipper leverFlipper;

    public LF_PummelState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName)
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
        if(isPummelOver)
        {
            leverFlipper.stateMachine.ChangeState(leverFlipper.grabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}