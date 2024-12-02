using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected float lastProcessedStunTime;
    protected bool isInLaunchVelcoity;

    protected float stunTime, knockback, angle;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stunTime = entity.currentHit.StunTime;
        knockback = entity.currentHit.Knockback;
        angle = entity.currentHit.Angle;
        isStunTimeOver = false;
        isMovementStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stunTime)
        {
            isStunTimeOver = true;
        }

        else if(isGrounded && Time.time> startTime + .2f && !isMovementStopped)
        {
            entity.SetVelocity(0);
            isMovementStopped = true;
            isStunTimeOver = true;
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
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isInLaunchVelcoity = entity.CheckIsInLaunchVelocity();
    }
}
