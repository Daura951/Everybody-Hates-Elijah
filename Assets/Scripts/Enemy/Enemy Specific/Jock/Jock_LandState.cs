using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_LandState : StunState
{
    private Jock jock;

    public Jock_LandState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Jock jock) : base(entity, stateMachine, animBoolName)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
