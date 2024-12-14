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

    //Player Movement
    public bool isRight;
    public bool isGrounded;
    public bool isRunning;
    public bool isOnMoveable;
    public bool OverrideControl;

    // Ledgegrab Bools
    public bool CanLedgeGrab;
    public bool isLedgeGrab;

    // Special Bool
    public bool CanUSpecial;
    public bool CanSSpecial;
    public bool isUSpecial = false;
    public bool isSSpecial = false;
    public bool isDSpecial = false;
    public bool isHelpless = false;

    public float Health, MaxHealth;


    private bool isHelplessCoroutine;
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
