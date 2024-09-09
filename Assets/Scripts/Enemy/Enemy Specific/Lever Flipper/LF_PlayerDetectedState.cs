using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_PlayerDetectedState : PlayerDetectedState
{
    private LeverFlipper leverFlipper;
    public LF_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName, stateData)
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

        }


        else
        {
            stateMachine.ChangeState(leverFlipper.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
