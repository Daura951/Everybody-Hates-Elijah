using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected D_IdleState stateData;
    protected bool flipAfterIdle;
    protected float idleTime;
    protected bool isIdleDone;
    protected bool isPlayerInMinAgroRange;
    protected bool isInCutscene;
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0);
        isIdleDone = false;
        SetRandomIdleTime();
        isInCutscene = entity.isInCutscene;
    }

    public override void Exit()
    {
        base.Exit();
        
        if(flipAfterIdle && !entity.isGrabbed && !entity.BB.isFrozen && !isInCutscene)
        {
            entity.Flip();
        }
    
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(Time.time >= startTime+idleTime && !entity.BB.isFrozen)
        {
            isIdleDone = true;
        }
    }

    public void setFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }

    public override void DoChecks()
    {
        isPlayerInMinAgroRange = entity.checkPlayerInMinAgroRange();
    }
}