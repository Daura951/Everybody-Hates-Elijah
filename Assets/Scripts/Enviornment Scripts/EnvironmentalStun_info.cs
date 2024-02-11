using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalStun_info : Stun_Info
{
    public EnvironmentHurtVals.AttackTypes attackType;
    // Start is called before the first frame update
    void Start()
    {
        CalculateStunVals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void CalculateStunVals()
    {
        string[] Info = EnvironmentHurtVals.environmentHurtValues[attackType].Split(" ");
        DAKTInfo = new float[4];
        DAKTInfo[0] = float.Parse(Info[0]); //Damage
        DAKTInfo[1] = float.Parse(Info[1]); //Angle
        DAKTInfo[2] = float.Parse(Info[2]); //Knockback
        DAKTInfo[3] = float.Parse(Info[3]); //Time
    }
}
