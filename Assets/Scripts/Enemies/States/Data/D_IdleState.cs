using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Enemy Data/State Data/Idle State")]
public class D_IdleState : ScriptableObject
{
    public float minIdleTime=1.0f, maxIdleTime=2.0f;
}
