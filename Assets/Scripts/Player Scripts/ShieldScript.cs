using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public GameObject Player;
    PlayerAttack PA;
    Animator ShieldAnim;
    Animator PAnim;
    public bool ShieldStun = false , ActiveOnce = false;
    public float ShieldTimer, StunTimer, SL;
    private float AVGShieldTime, StoredShieldTime, StoredStunTime;
    [Header("Shield Fraction Damage")]
    public float lowfrac, medfrac, highfrac;


    // Start is called before the first frame update
    void Awake()
    {
        PA = Player.GetComponent<PlayerAttack>();
        ShieldAnim = GetComponent<Animator>();
        PAnim=Player.GetComponent<Animator>();
        ActiveOnce = true;
        StoredShieldTime = ShieldTimer - 1f;
        StoredStunTime = StunTimer - 1f;
        AVGShieldTime = (1f - SL) / ShieldTimer;
        ShieldTimer = 0;
        StunTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShieldTimer > StoredShieldTime)
            ShieldTimer = StoredShieldTime;





        if (PA.shielding && !ShieldStun)
        {

            if (ShieldTimer < StoredShieldTime)
            {
                ShieldTimer += Time.deltaTime;
                float scale = 1f - (AVGShieldTime * ShieldTimer);
                transform.localScale = new Vector3(scale, scale, 1f);
            }
            else
                ShieldTimer = StoredShieldTime;
            if (ShieldTimer == StoredShieldTime)
            {
                StunTimer = 0;
                ShieldStun = true;
                PA.shielding = false;
                ShieldAnim.Play("ShieldBreak");
                PAnim.Play("ShieldPop");
            }
        }

        if (ShieldStun)
        {
            if (StunTimer < StoredStunTime)
            {
                StunTimer += Time.deltaTime;
            }
            else
                StunTimer = StoredStunTime;
            if (StunTimer == StoredStunTime)
            {
                ShieldStun = false;
                PA.ShieldOff();
            }
        }
    }


    public void ShieldDamag(int x)
    {
        switch(x)
        {
            case 1:
            ShieldTimer += StoredShieldTime * lowfrac;
            break;

            case 2:
            ShieldTimer += StoredShieldTime * medfrac;
            break;

            case 3:
            ShieldTimer += StoredShieldTime * highfrac;
            break;
        }
    }
}
