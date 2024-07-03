using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;

    public void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    public void FinishAtack()
    {
        if(attackState!=null)
        attackState.FinishAttack();
    }
}
