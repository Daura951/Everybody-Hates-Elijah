using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_JumpState : JumpState
{
    private Jock jock;
    public Jock_JumpState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_JumpState stateData, Jock jock) : base(entity, stateMachine, animBoolName , stateData)
    {
        this.jock = jock;
    }

    public override void Enter()
    {
        base.Enter();
        if (entity.checkPassThroughAbove())
        {
        entity.SetVelocity(0);
        }
        entity.rb.AddForce(new Vector2(0f,jumpforce), ForceMode2D.Impulse);
        jock.idleState.setFlipAfterIdle(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        entity.rb.AddForce(Vector2.right *entity.facingDir);

        if(!entity.CheckWall())
            jock.idleState.setFlipAfterIdle(false);

        if (entity.rb.velocity.y == 0)
            stateMachine.ChangeState(jock.idleState);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
