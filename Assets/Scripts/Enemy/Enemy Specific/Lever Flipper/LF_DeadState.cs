using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_DeadState : DeadState
{

    private LeverFlipper leverFlipper;

    public LF_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, LeverFlipper leverFlipper) : base(entity, stateMachine, animBoolName)
    {
        this.leverFlipper = leverFlipper;
    }
}
