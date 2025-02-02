using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetails
{

    private Attacks attack;
    private float damage;
    private float angle;
    private float knockback;
    private float stunTime;
    private float freezeDuration;

    public AttackDetails(Attacks attack, float damage, float angle, float knockback, float stunTime, float freezeDuration)
    {
        this.Attack = attack;
        this.Damage = damage;
        this.Angle = angle;
        this.Knockback = knockback;
        this.StunTime = stunTime;
        this.freezeDuration = freezeDuration;
    }

    public AttackDetails(float damage, float angle, float knockback, float stunTime)
    {
        this.Attack = Attacks.ENEMY_HIT;
        this.Damage = damage;
        this.Angle = angle;
        this.Knockback = knockback;
        this.StunTime = stunTime;
    }


    public override string ToString()
    {
        return Attack.ToString() + ": " + Damage + " " + Angle + " " + Knockback + " " + StunTime + " "+freezeDuration;
    }
    public Attacks Attack { get => attack; set => attack = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Angle { get => angle; set => angle = value; }
    public float Knockback { get => knockback; set => knockback = value; }
    public float StunTime { get => stunTime; set => stunTime = value; }
    public float FreezeDuration { get => freezeDuration; set => freezeDuration = value; }


}

public enum Attacks
{
    JAB_1,
    JAB_2,
    JAB_3,
    FTILT,
    UTILT,
    DTILT,
    NAIR,
    FAIR,
    UAIR,
    DAIR,
    BAIR,
    FSTRONG,
    USTRONG,
    DSTRONG,
    NSPECIAL,
    FSPECIAL,
    USPECIAL_1,
    USPECIAL_2,
    USPECIAL_3,
    DSPECIAL_1,
    DSPECIAL_2,
    DSPECIAL_3,
    DASH,
    FAIR_2,
    GRAB,
    ENEMY_HIT
}
