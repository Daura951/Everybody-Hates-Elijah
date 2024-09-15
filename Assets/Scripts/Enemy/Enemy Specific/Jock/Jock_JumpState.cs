using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_JumpState : JumpState
{
    private Jock jock;
    bool Hordir;
    float Y;
    public Jock_JumpState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_JumpState stateData, Jock jock) : base(entity, stateMachine, animBoolName , stateData)
    {
        this.jock = jock;
    }

    public override void Enter()
    {
        base.Enter();

        jock.idleState.setFlipAfterIdle(false);
        Hordir = !entity.checkLedge();
        entity.SetVelocity(0);
        Y = entity.transform.position.y;

        if(!Hordir)
        entity.rb.AddForce(new Vector2(0,jumpforce+.475f), ForceMode2D.Impulse);
        else
            entity.rb.AddForce(new Vector2(jumpdis, jumpforce*.5f), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        if (!Hordir && (entity.transform.position.y <= Y+.1f && entity.transform.position.y >= Y-.1f))
            jock.idleState.setFlipAfterIdle(true);

        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!Hordir)
        {
         entity.rb.AddForce(Vector2.right *entity.facingDir * 2f);
        }

         if (entity.rb.velocity.y == 0)
         stateMachine.ChangeState(jock.idleState);


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
