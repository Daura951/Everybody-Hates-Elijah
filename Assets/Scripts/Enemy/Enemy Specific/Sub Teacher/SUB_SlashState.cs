using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_SlashState : MeleeAttackState
{
    private SubTeacher subTeacher;
    public SUB_SlashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.subTeacher = subTeacher;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        hitBox = subTeacher.hitboxes[stateData.hitBoxIndex];

        if(entity.isGrabbed)
        {
            stateMachine.ChangeState(subTeacher.idleState);
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

        if(isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(subTeacher.playerDetectedState);
            }
            else stateMachine.ChangeState(subTeacher.lookForPlayerState);
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
