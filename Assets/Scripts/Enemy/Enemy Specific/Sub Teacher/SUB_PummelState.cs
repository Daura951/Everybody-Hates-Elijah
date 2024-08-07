using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_PummelState : PummelState
{
    private SubTeacher subTeacher;

    public SUB_PummelState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName)
    {
        this.subTeacher = subTeacher;
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
            subTeacher.stateMachine.ChangeState(subTeacher.grabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}