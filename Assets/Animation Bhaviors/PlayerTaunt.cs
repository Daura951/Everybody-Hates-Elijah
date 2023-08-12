using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTaunt : MonoBehaviour
{
    public static PlayerTaunt TauntInstance;
    public Animator anim;
    public PlayerMovement PM;
    private float H;
    public string taunt;

     private void Awake()
    {
        TauntInstance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxisRaw("TauntH") > 0 &&  Input.GetAxisRaw("TauntV") == 0 && Input.GetAxisRaw("Vertical") == 0 && string.Equals(taunt , "") && anim.GetBool("Idle") == true && !PM.isInAir)
        {
            print("Right");
            taunt = "Right";
        }

        else if (Input.GetAxisRaw("TauntH") < 0 && Input.GetAxisRaw("TauntV") == 0 && Input.GetAxisRaw("Vertical") == 0 && string.Equals(taunt , "") && anim.GetBool("Idle") == true && !PM.isInAir)
        {
            print("Left");
            taunt = "Left";
        }

        else if (Input.GetAxisRaw("TauntV") > 0 && Input.GetAxisRaw("TauntH") == 0 && Input.GetAxisRaw("Vertical") == 0 && string.Equals(taunt , "") && anim.GetBool("Idle") == true && !PM.isInAir)
        {
            print("Up");
            taunt = "Up";
        }

        else if (Input.GetAxisRaw("TauntV") < 0 && Input.GetAxisRaw("TauntH") == 0 && Input.GetAxisRaw("Vertical") == 0 && string.Equals(taunt , "") && anim.GetBool("Idle") == true && !PM.isInAir)
        {
            print("Down");
            taunt = "Down";
        }
    }

    public void ResetTaunt()
    {
        taunt = "";
    }
}
