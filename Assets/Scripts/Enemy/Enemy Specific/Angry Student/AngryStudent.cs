using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryStudent : Entity
{
    public AS_IdleState idleState { get; private set; }
    public AS_MoveState moveState { get; private set; }
    public AS_PlayerDetectedState playerDetectedState { get; private set;}
    public AS_RunAtState runAtState { get; private set; }
    public AS_LookForPlayerState lookForPlayerState { get; private set; }

    public AS_DeadState deadState;
    
    public AS_PunchState punchState { get; private set; }

    public AS_StunState stunState { get; private set; }

    public AS_GrabState grabState { get; private set; }

    public AS_PummelState pummelState { get; private set; }

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
        if( 1 == Random.Range(1, 3))
        {
            isMale = false;
            Vocals.Replace();
            anim.runtimeAnimatorController = AOC;
        }
        Vocals.RoleCall();
        moveState = new AS_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new AS_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new AS_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        runAtState = new AS_RunAtState(this, stateMachine, "runAt", runAtStateData, this);
        lookForPlayerState = new AS_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        punchState = new AS_PunchState(this, stateMachine, "punch", punchPosition,punchStateData, this);
        stunState = new AS_StunState(this, stateMachine, "stun", this);
        deadState = new AS_DeadState(this, stateMachine, "dead", this);
        grabState = new AS_GrabState(this, stateMachine, "grabbed", this);
        pummelState = new AS_PummelState(this, stateMachine, "pummel", this);
       
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

    public  override void OnDrawGizmos()
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

    public override void GetGrabbed()
    {
        base.GetGrabbed();
        stateMachine.ChangeState(grabState);

    }

    public override void GetPummeled()
    {
        base.GetPummeled();
        stateMachine.ChangeState(pummelState);
        
    }

    public override void getThrown()
    {
        base.getThrown();
        stateMachine.ChangeState(stunState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" && stateMachine.currentState == stunState || stateMachine.currentState == deadState)
        {
            SetVelocity(0);

            if(health.GetHealth() > 0)
                stateMachine.ChangeState(idleState);
        }
    }

}
