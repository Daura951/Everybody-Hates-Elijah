using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackDetails
{

    private Attacks attack;
    private float damage;
    private float angle;
    private float knockback;
    private float stunTime;

    public PlayerAttackDetails(Attacks attack, float damage, float angle, float knockback, float stunTime)
    {
        this.attack = attack;
        this.damage = damage;
        this.angle = angle;
        this.knockback = knockback;
        this.stunTime = stunTime;
    }

    public override string ToString()
    {
        return attack.ToString() + ": " + damage + " " + angle + " " + knockback + " " + stunTime;
    }
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
  //  NSPECAIL,
    FSPECIAL,
    USPECIAL,
    DSPECIAL_1,
    DSPECIAL_2,
    DSPECIAL_3,
}
