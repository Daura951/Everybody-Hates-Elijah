using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_SpinState : StunState
{
    private AngryStudent angryStudent;

    public AS_SpinState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
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

        if (entity.anim.GetBool("Grounded"))
        {
            stateMachine.ChangeState(angryStudent.landState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
