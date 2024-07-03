using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDist = .2f;
    public float ledgeCheckDist = 1f;
    public float groundCheckRadius = .3f;
    public LayerMask whatIsGround;
    public LayerMask endLedge;
    public float minAgroDist = 3f;
    public float maxAgroDist = 4f;
    public LayerMask whatIsPlayer;
    public float closeRangeAttackDist = 2f;
    public float grabbedTime = 3.5f;
    public float pummelStateTime = 0.05f;
    public int maxPummels= 7;
}