using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock : Entity
{
    public Jock_IdleState idleState { get; private set; }
    public Jock_MoveState moveState { get; private set; }
    public Jock_PlayerDetectedState playerDetectedState { get; private set; }
    public Jock_RunAtState runAtState { get; private set; }
    public Jock_LookForPlayerState lookForPlayerState { get; private set; }

    public Jock_PunchState punchState { get; private set; }

    public Jock_DeadState deadState;

    public Jock_StunState stunState { get; private set; }


    public GameObject[] hitboxes;

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetected playerDetectedData;
    [SerializeField] private D_RunAtState runAtStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerData;
    [SerializeField] private D_MeleeAttack punchStateData;
    [SerializeField] private Transform punchPosition;
    [SerializeField] private AnimatorOverrideController AOC;


    public override void Start()
    {
        base.Start();
        if (1 == Random.Range(1, 3) && AOC != null)
        {
            isMain = false;
            Vocals.Replace();
            anim.runtimeAnimatorController = AOC;
        }
        moveState = new Jock_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Jock_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Jock_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        runAtState = new Jock_RunAtState(this, stateMachine, "runAt", runAtStateData, this);
        lookForPlayerState = new Jock_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        punchState = new Jock_PunchState(this, stateMachine, "punch", punchPosition, punchStateData, this);
        stunState = new Jock_StunState(this, stateMachine, "stun", this);
        deadState = new Jock_DeadState(this, stateMachine, "dead", this);

        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        if (!isInCutscene)
        {
            base.Update();

            if (health.GetHealth() <= 0)
            {
                stateMachine.ChangeState(deadState);
            }

        }

        else
        {
            stateMachine.ChangeState(idleState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void Damage()
    {
        base.Damage();
        if (!BB.isFrozen)
        {
            if (stateMachine.currentState != stunState && health.GetHealth() > 0)
            {
                stateMachine.ChangeState(stunState);
            }
            else if (health.GetHealth() <= 0)
            {
                stateMachine.ChangeState(deadState);
            }
       }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" && stateMachine.currentState == stunState || stateMachine.currentState == deadState)
        {
            SetVelocity(0);

            if (health.GetHealth() > 0)
                stateMachine.ChangeState(idleState);
        }
    }

}

