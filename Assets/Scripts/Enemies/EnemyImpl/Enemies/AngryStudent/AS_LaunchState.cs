using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_LaunchState : LaunchState
{
    private AngryStudent angryStudent;

    public AS_LaunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
    {
        this.angryStudent = angryStudent;
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
        if(isLaunchOver && !angryStudent.isDead)
        {
            stateMachine.ChangeState(angryStudent.getupState);
        }
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
