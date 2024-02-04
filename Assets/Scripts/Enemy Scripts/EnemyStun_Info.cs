using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStun_Info : Stun_Info
{
    public EnemyHurtVals.AttackTypes attackType;
    void Start()
    {
        CalculateStunVals();
    }
    public override void CalculateStunVals()
    {
        string[] Info = EnemyHurtVals.maleStudentAttackValues[attackType].Split(" ");
        DAKTInfo = new float[4];
        DAKTInfo[0] = float.Parse(Info[0]); //Damage
        DAKTInfo[1] = float.Parse(Info[1]); //Angle
        DAKTInfo[2] = float.Parse(Info[2]); //Knockback
        DAKTInfo[3] = float.Parse(Info[3]); //Time
    }
}
