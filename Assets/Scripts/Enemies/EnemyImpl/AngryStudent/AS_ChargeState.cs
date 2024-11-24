using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_ChargeState : ChargeState
{
    private AngryStudent angryStudent;
    public AS_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.angryStudent = angryStudent;
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

        /*
         * If the Elijah is in the enemy's line of sight, keep charging until Elijah is out of sight, or in the enemy's attack range
         */

        if(!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(angryStudent.lookForPlayerState); 
        }

        else if (performCloseRangeAction)
        {
            int attackChoice = Random.Range(1, 3);
            stateMachine.ChangeState((attackChoice == 1 ? angryStudent.punchState : angryStudent.kickState));
        }

        else if(!isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(angryStudent.lookForPlayerState);
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
