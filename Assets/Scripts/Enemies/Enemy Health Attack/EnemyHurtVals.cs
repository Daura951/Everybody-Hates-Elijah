using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtVals : MonoBehaviour
{
    public enum AttackTypes
    {
        Punch, Kick, //angry student
        Charge,      // Jock
        Slash, Boomerang //sub teacher
    };

    public static  Dictionary<AttackTypes, string> attackValues = new Dictionary<AttackTypes, string>
    {
        //TODO: Tsering and Rob, pls fill these out proper!
        //Angry student
        {AttackTypes.Punch, "5 60 25 .3" },
        {AttackTypes.Kick, "3 70 10 .2" },

        //Jock
        {AttackTypes.Charge, "10 90 20 .7" },

        //Sub Teacher
        {AttackTypes.Slash, "5 30 10 .3" },
        {AttackTypes.Boomerang, "10 60 10 .3" }
    };

    public static AttackDetails convertDictValToAttackDetails(AttackTypes attack)
    {
        string[] parts = EnemyHurtVals.attackValues[attack].Split(' ');
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
