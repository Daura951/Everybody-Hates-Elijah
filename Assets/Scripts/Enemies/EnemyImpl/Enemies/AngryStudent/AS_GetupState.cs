using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_GetupState : GetupState
{
    private AngryStudent angryStudent;

    public AS_GetupState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
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

        if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("Male Getup") && entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            stateMachine.ChangeState(angryStudent.idleState);
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
