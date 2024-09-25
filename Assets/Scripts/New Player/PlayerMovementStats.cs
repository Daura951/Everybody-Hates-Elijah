using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(1f, 100f)] public float MaxWalkSpeed = 7f;
    [Range(0.25f, 50f)] public float GroundAccel = 5f;
    [Range(0.25f, 50f)] public float GroundDecel = 20f;
    [Range(0.25f, 50f)] public float AirAccel = 5f;
    [Range(0.25f, 50f)] public float AirDecel = 5f;

    [Header("Run")]
    [Range(1f, 100f)] public float MaxRunSpeed = 14f;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public bool ShowGroundBox;
    public float GroundDetectLength = 0.02f;
    public float HeadDetectLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = .75f;

    [Header("Jump")]
    public float JumpHieght = 6.5f;
    [Range(1f,1.1f)] public float JumpHieghtCompFactor = 1.054f;
    public float JumpApexTime = 0.35f;
    [Range(0.01f, 5f)] public float GravityReleaseMultiplyer = 2f;
    public float MaxFallSpeed = 26f;
    [Range(1, 5)] public float NumberofJumps = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float UpCancelTime = 0.027f;

    [Header("Jump Apex")] 
    [Range(0.5f, 1f)] public float ApexThreshold = 0.97f;
    [Tooltip("Time that Elijah stays at the Arc Peak")]
    [Range(0.01f, 1f)] public float ApexhangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f;

    [Header("Jump Visualization")]
    public bool WalkJumpArc = false;
    public bool RunJumpArc = false;
    public bool StopOncollision = true;
    public bool DrawRight = true;
    [Range(5, 100)] public int ArcRes = 20;
    [Range(0, 500)] public int VisualSteps = 90;

    public float Gravity { get; private set; }
    public float InitialJumpVelo { get; private set; }
    public float AdjustedJumpHieght { get; private set; }

    private void OnValidate()
    {
        CalcValues();
    }

    private void OnEnable()
    {
        CalcValues();
    }

    private void CalcValues()
    {
        AdjustedJumpHieght = JumpHieght * JumpHieghtCompFactor;
        Gravity = -(2f * AdjustedJumpHieght) / Mathf.Pow(JumpApexTime, 2f);
        InitialJumpVelo = Mathf.Abs(Gravity) * JumpApexTime;

    }

}
