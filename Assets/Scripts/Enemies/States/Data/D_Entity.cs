using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName ="Enemy Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance=0.2f, ledgeCheckDistance=0.4f;

    public float minAgroDistance = 3f, maxAgroDistance = 4f;

    public LayerMask whatIsPlayer;

    public LayerMask whatIsGround;

    public float closeRangeActionDistance = 1f;

    public float maxHealth = 30;

    public float damageHopVelocity = 3;

    public float groundCheckRadius = 0.3f;

    public float launchVelocityThreshold = 10f;

    public float maxStunTime = 4.0f;
    public float getupTime = 2.0f;

    public float deathTime = 1.0f;

}
