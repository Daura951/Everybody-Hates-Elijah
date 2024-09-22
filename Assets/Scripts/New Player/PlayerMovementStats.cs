using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(1f, 100f)] public float MaxWalkSpeed = 7f;
    [Range(0.25f, 50f)] public float GroundAccel= 5f;
    [Range(0.25f, 50f)] public float GroundDecel= 20f;
    [Range(0.25f, 50f)] public float AirAccel= 5f;
    [Range(0.25f, 50f)] public float AirDecel= 5f;

    [Header("Run")]
    [Range(1f, 100f)] public float MaxRunSpeed = 14f;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectLength = 0.02f;
    public float HeadDetectLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = .75f;
}
