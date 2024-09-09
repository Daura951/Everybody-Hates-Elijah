using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpStateData", menuName = "Data/State Data/Jump State")]
public class D_JumpState : ScriptableObject
{
    public GameObject projectile;
    public float JumpForce = 5;
    public float JumpDistance = 5;
    public float jumpHeight = 5;
}

