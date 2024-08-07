using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtVals : MonoBehaviour
{
    public enum AttackTypes
    {
        Punch, Kick, //angry student
        Slash, Boomerang //sub teacher
    };

    public static  Dictionary<AttackTypes, string> attackValues = new Dictionary<AttackTypes, string>
    {
        //Angry student
        {AttackTypes.Punch, "5 30 10 .3" },
        {AttackTypes.Kick, "3 20 5 .2" },

        //Sub Teacher
        {AttackTypes.Slash, "5 30 10 .3" },
        {AttackTypes.Boomerang, "10 60 10 .3" }
    };
}
