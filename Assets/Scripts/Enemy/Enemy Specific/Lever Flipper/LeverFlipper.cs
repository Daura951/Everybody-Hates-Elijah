using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverFlipper: Entity
{
    public LF_IdleState idleState { get; private set; }

    public LF_PullIdleState pullIdleState { get; private set; }

    public LF_AwayFromLeverIdle awayFromLeverIdleState { get; private set; }
    public LF_DeadState deadState { get; private set; }
    public LF_StunState stunState { get; private set; }
    public LF_GrabState grabState { get; private set; }
    public LF_PummelState pummelState { get; private set; }

    public LF_PullState pullState;
    

    [SerializeField] private D_IdleState idleStateData;
    public GameObject spikeTrap;
    public GameObject lever;

    //accounts for if he is away from lever OR if he pulls it. It has 2 meanings
    public bool isLeverPulled = false;

    public Transform playerCheckTFT;

    public override void Start()
    {
        base.Start();
        idleState = new LF_IdleState(this, stateMachine, "idle", idleStateData, this);
        pullIdleState = new LF_PullIdleState(this, stateMachine, "pIdle", idleStateData, this);
        awayFromLeverIdleState = new LF_AwayFromLeverIdle(this, stateMachine, "aflIdle" ,idleStateData, this);
        stunState = new LF_StunState(this, stateMachine, "stun", this);
        deadState = new LF_DeadState(this, stateMachine, "dead", this);
        grabState = new LF_GrabState(this, stateMachine, "grabbed", this);
        pummelState = new LF_PummelState(this, stateMachine, "pummel", this);
        pullState = new LF_PullState(this, stateMachine, "pull", null, null, this);

        lever.transform.parent = null;
        spikeTrap.transform.parent = null;

        stateMachine.Init(idleState);
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
        Flip();
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

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag == "Platform" && stateMachine.currentState == stunState || stateMachine.currentState == deadState)
        {

            if(health.GetHealth() > 0)
                stateMachine.ChangeState(idleState);
        }
    }
}
