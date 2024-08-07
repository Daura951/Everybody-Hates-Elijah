using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_GrabState : GrabState
{
    private SubTeacher subTeacher;

    public SUB_GrabState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName)
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

        if(isFree)
        {
            stateMachine.ChangeState(subTeacher.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}