using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    protected D_JumpState stateData;
    protected bool isDetectingWall, isDetectingLedge;
    protected float jumpforce;
    protected float jumpdis;
    protected bool isPlayerInMinAgroRange;

    public JumpState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_JumpState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        jumpforce = Mathf.Sqrt(stateData.jumpHeight * -2 * (Physics2D.gravity.y * entity.rb.gravityScale));
        jumpdis = entity.facingDir * (stateData.JumpDistance + entity.transform.localScale.x);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.checkLedge();
        isDetectingWall = entity.CheckWall();
    }
}
