using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_PummelState : PummelState
{
    private AngryStudent angryStudent;

    public AS_PummelState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
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
        if(isPummelOver)
        {
            angryStudent.stateMachine.ChangeState(angryStudent.grabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}