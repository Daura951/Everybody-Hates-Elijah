using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_PunchState : MeleeAttackState
{
    private Jock jock;
    public Jock_PunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Jock jock) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.jock = jock;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        hitBox = jock.hitboxes[stateData.hitBoxIndex];

        if (entity.isGrabbed)
        {
            stateMachine.ChangeState(jock.idleState);
        }
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

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(jock.playerDetectedState);
            }
            else stateMachine.ChangeState(jock.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
