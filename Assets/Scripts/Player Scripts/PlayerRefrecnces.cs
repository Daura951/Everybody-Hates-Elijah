using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRefrecnces : MonoBehaviour
{

    public static PlayerRefrecnces instance;


    public PlayerAttack Attack;
    public PlayerMovement Move;
    public PlayerTaunt Taunt;
    public Stun Stun;
    public Rigidbody2D RB;
    public Animator anim;


    void Start()
    {
        instance = this;
    }
}


