using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_PullIdleState : IdleState
{
    private LeverFlipper leverFlipper;
    public LF_PullIdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.leverFlipper = leverFlipper;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}