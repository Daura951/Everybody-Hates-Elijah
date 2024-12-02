using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetupState : State
{
    public GetupState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
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
