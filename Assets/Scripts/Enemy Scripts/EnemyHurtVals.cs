using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtVals
{
    public enum AttackTypes
    {
        Punch, Kick
    };

    public static  Dictionary<AttackTypes, string> maleStudentAttackValues = new Dictionary<AttackTypes, string>
    {
        {AttackTypes.Punch, "5 30 10 .3" },
        {AttackTypes.Kick, "3 20 5 .2" }
    };
}
