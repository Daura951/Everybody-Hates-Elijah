using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_IdleState : IdleState
{
    private LeverFlipper leverFlipper;
    public LF_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.leverFlipper = leverFlipper;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!entity.BB.isFrozen)
        {

            if(leverFlipper.isLeverPulled)
            {
                stateMachine.ChangeState(leverFlipper.awayFromLeverIdleState);
            }

            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(leverFlipper.pullState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}