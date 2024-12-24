using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    public bool isAttacking;
    public bool isBBReady;
    public bool isInBB;
    public bool isInCutscene;
    public float Health, MaxHealth;

    //Player Movement
    public bool isRight;
    public bool isGrounded;
    public bool isRunning;
    public bool isOnMoveable;

    // Ledgegrab Bools
    public bool CanLedgeGrab;
    public bool isLedgeGrab;

    // Stun bools
    public bool isStunned;      //Player is stunned and cannot move
    public bool VelocityStunned; //Player is no longer stunned but is traveling a stun path

    // Special Bool
    public bool CanUSpecial;
    public bool CanSSpecial;
    public bool isUSpecial = false;
    public bool isSSpecial = false;
    public bool isDSpecial = false;
    public bool isHelpless = false;


    [SerializeField]
    private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        render.color = isHelpless ? Color.grey : new Color(255f, 255f, 255f, 255f);

    }

}
