using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_SpinState : StunState
{
    private Jock jock;

    public Jock_SpinState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Jock jock) : base(entity, stateMachine, animBoolName)
    {
        this.jock = jock;
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

        if (entity.anim.GetBool("Grounded"))
        {
            stateMachine.ChangeState(jock.landState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
