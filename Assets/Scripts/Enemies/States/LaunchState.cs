using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchState : State
{
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool isLaunchOver;
    public LaunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isMovementStopped = false;
        isLaunchOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isGrounded && Time.time > startTime + .2f && !isMovementStopped)
        {
            entity.SetVelocity(0);
            isMovementStopped = true;
            isLaunchOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = entity.CheckGround();
    }

}
