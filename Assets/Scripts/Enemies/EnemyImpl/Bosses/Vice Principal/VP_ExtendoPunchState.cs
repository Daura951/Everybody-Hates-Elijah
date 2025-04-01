using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VP_ExtendoPunchState : MeleeAttackState
{
    private VicePrincipal vp;

    public VP_ExtendoPunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        this.entity = vp;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
