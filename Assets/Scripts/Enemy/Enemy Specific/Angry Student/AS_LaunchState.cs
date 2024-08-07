using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_LaunchState : StunState
{
    private AngryStudent angryStudent;

    public AS_LaunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
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

        if (entity.Yspeed < -.035f && !entity.CheckGround())
        {
            stateMachine.ChangeState(angryStudent.spinState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
