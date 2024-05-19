using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_PunchState : MeleeAttackState
{
    private AngryStudent angryStudent;
    public AS_PunchState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.angryStudent = angryStudent;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        hitBox = angryStudent.hitboxes[stateData.hitBoxIndex];
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
                stateMachine.ChangeState(angryStudent.playerDetectedState);
            }
            else stateMachine.ChangeState(angryStudent.lookForPlayerState);
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
