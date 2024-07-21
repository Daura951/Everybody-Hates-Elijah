using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerPrefs.SetInt("loadGameIndex", SceneManager.GetActiveScene().buildIndex);
    }
}


