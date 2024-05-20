using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{

    protected float stunTime;
    protected bool isStunOver;
    protected bool isGrounded;
    protected bool performCloseRangeAttack;
    protected bool isPlayerInMinAgroRange;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = entity.CheckGround();
        performCloseRangeAttack = entity.checkPlayerInCloseRangeAttack();
        isPlayerInMinAgroRange = entity.checkPlayerInMinAgroRange();
    
    }

    public override void Enter()
    {
        base.Enter();
        isStunOver = false;
        if(entity.stats != null)
        {
            stunTime = entity.stats[3];
        }
        else stunTime = 0.01f;
        
        entity.rb.gravityScale = 1f;
        entity.playerGO.GetComponent<Animator>().SetBool("hasGrabbedEnemy", false);
        entity.pummelFactor = 0;
    }

    public override void Exit()
    {
        base.Exit();
        isStunOver = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime+stunTime)
        {
            isStunOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}