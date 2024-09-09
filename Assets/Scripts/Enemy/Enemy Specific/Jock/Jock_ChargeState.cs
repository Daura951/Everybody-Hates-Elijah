using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_ChargeState : MeleeAttackState
{
    private Jock jock;
    protected bool isRunAtDone;
    public Jock_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Jock jock) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.jock = jock;
    }

    public override void Enter()
    {
        base.Enter();
        hitBox = jock.hitboxes[stateData.hitBoxIndex];
        entity.SetVelocity(jock.RunData().runAtSpeed);
        isRunAtDone = false;
        hitBox.SetActive(true);
        entity.Vocals.Attack();
    }

    public override void Exit()
    {
        base.Exit();
        hitBox.SetActive(false);
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + jock.RunData().runAtTime)
        {
            isRunAtDone = true;
        }


        if (!entity.checkLedge() || entity.CheckWall())
        {
            stateMachine.ChangeState(jock.idleState);
        }
       else if (isRunAtDone)
       {
            stateMachine.ChangeState(jock.chargeFState);
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
