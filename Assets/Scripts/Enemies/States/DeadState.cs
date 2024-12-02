using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + entity.entityData.deathTime)
        {
            entity.gameObject.SetActive(false);
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
