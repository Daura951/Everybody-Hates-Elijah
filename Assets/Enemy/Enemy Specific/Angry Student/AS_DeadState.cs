using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_DeadState : DeadState
{

    private AngryStudent angryStudent;

    public AS_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, AngryStudent angryStudent) : base(entity, stateMachine, animBoolName)
    {
        this.angryStudent = angryStudent;
    }
}
