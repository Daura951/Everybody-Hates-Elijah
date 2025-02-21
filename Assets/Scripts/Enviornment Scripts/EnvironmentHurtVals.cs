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
            {AttackTypes.Lava, "5 120 160 .5" },
            {AttackTypes.Death, "100000000 0 0 0" },
            {AttackTypes.techKidProjectile, "5 45 50 .2" }
        };

    public static AttackDetails convertDictValToAttackDetails(AttackTypes attack)
    {
        string[] parts = environmentHurtValues[attack].Split(' ');
        float[] values = new float[parts.Length];

        for (int j = 0; j < parts.Length; j++)
        {
            if (float.TryParse(parts[j], out float result))
            {
                values[j] = result;
            }

        }
        return new AttackDetails(values[0], values[1], values[2], values[3]);
    }

}
