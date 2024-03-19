using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHurtVals
{
    public enum AttackTypes
    {
        Lava, Death
    };
    // Damage , Angle , Knockback, time
    public static Dictionary<AttackTypes, string> environmentHurtValues = new Dictionary<AttackTypes, string>
        {
            {AttackTypes.Lava, "5 90 70 .5" },
            {AttackTypes.Death, "100000000 0 0 0" }
        };

}
