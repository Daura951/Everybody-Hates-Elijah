using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{

    public bool isAttacking;
    public bool isRight;
    public bool isGrounded;
    public bool isRunning;
    public bool isBBReady;
    public bool isInBB;
    public bool isLedgeGrab;
    public bool isInCutscene;
    public bool isOnMoveable;


    public bool CanUSpecial;
    public bool CanSSpecial;
    public bool isUSpecial = false;
    public bool isSSpecial = false;
    public bool isDSpecial = false;

    public float Health, MaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }
}
