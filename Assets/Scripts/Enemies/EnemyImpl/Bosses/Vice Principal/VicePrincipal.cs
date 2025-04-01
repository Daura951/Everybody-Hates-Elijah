using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicePrincipal : Entity
{

    public VP_IdleState idleState { get; private set; }
    public VP_WalkState walKState { get; private set; }
    public VP_ExtendoPunchState epState { get; private set; }
    public VP_GroundPunchState gPunchState { get; private set; }
    public VP_YellState yellState { get; private set; }
    public VP_DTSBTState dtsbtState { get; private set; }

    [SerializeField]
    private D_IdleState idleData;
    [SerializeField]
    private D_MoveState moveData;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        idleState = new VP_IdleState(this, stateMachine, "idle", idleData);
        walKState = new VP_WalkState(this, stateMachine, "walk", moveData);
        epState = new VP_ExtendoPunchState(this, stateMachine, "epunch");
        gPunchState = new VP_GroundPunchState(this, stateMachine, "gpunch");
        yellState = new VP_YellState(this, stateMachine, "yell");
        dtsbtState = new VP_DTSBTState(this, stateMachine, "dtsbt");
        stateMachine.Initalize(idleState);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
