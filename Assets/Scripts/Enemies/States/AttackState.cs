using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
        entity.isAnimationFinished = false;
        isAnimationFinished = entity.isAnimationFinished;
    }

    public override void Exit()
    {
        base.Exit();
        entity.isAnimationFinished = false;
        isAnimationFinished = entity.isAnimationFinished;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        isAnimationFinished = entity.isAnimationFinished;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange(); 
    }

}
