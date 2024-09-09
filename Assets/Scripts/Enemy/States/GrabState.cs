using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : State
{
    protected float grabbedTime;
    protected bool isFree;
    protected int maxPummels;
    public GrabState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        grabbedTime = entity.entityData.grabbedTime;
        entity.SetVelocity(0);
        entity.rb.gravityScale = 0;
        isFree = false;
        maxPummels = entity.entityData.maxPummels;
        entity.CurrentlyGrabbed();
    }

    public override void Exit()
    {
        base.Exit();
        if (isFree)
        {
            entity.rb.gravityScale = 1f;
            entity.pummelFactor = 0;
            entity.playerGO.GetComponent<Animator>().SetBool("hasGrabbedEnemy", false);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > startTime + grabbedTime || entity.pummelFactor > maxPummels)
        {
            isFree = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
