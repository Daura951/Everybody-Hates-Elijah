using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_PullState : MeleeAttackState
{
    private LeverFlipper leverFlipper;
    public LF_PullState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.leverFlipper = leverFlipper;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        leverFlipper.isLeverPulled = true;
        leverFlipper.spikeTrap.SetActive(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(leverFlipper.isLeverPulled)
        {
            stateMachine.ChangeState(leverFlipper.awayFromLeverIdleState);
        }

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(leverFlipper.pullIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        leverFlipper.lever.GetComponent<Animator>().Play("Pull Down");
    }
}
