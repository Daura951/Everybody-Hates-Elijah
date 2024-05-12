using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeStateData", menuName = "Data/State Data/Melee State")]
public class D_MeleeAttack : ScriptableObject
{
    public float attackRadius = 2f;
    public LayerMask whatIsPlayer;
    public float damage = 10;
    public int hitBoxIndex = 0;
}