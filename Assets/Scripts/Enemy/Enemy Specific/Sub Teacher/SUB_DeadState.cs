using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUB_DeadState : DeadState
{

    private SubTeacher subTeacher;

    public SUB_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, SubTeacher subTeacher) : base(entity, stateMachine, animBoolName)
    {
        this.subTeacher = subTeacher;
    }
}
