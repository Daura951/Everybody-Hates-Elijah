using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PummelState : State
{

    protected float pummelStateTime;
    protected bool isPummelOver;
    protected int maxPummelAmt;
    public PummelState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        pummelStateTime = entity.entityData.pummelStateTime;
        isPummelOver = false;
        maxPummelAmt = entity.entityData.maxPummels;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > startTime+pummelStateTime || entity.pummelFactor > maxPummelAmt)
        {
            isPummelOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}