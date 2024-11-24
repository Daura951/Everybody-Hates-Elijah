using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHurtVals
{
    public enum AttackTypes
    {
        Lava, Death, techKidProjectile
    };
    // Damage , Angle , Knockback, time
    public static Dictionary<AttackTypes, string> environmentHurtValues = new Dictionary<AttackTypes, string>
        {
            {AttackTypes.Lava, "5 100 70 1" },
            {AttackTypes.Death, "100000000 0 0 0" },
            {AttackTypes.techKidProjectile, "5 45 50 .2" }
        };

}
