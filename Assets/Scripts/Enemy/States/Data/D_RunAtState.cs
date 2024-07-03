using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRunAtStateData", menuName = "Data/State Data/Run At State")]
public class D_RunAtState : ScriptableObject
{
    public float runAtSpeed = 6f;

    public float runAtTime = 1f;
}