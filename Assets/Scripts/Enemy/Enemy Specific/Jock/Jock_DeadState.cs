using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jock_DeadState : DeadState
{

    private Jock jock;

    public Jock_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Jock jock) : base(entity, stateMachine, animBoolName)
    {
        this.jock = jock;
    }
}
