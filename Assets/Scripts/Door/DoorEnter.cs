using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    PlayerMovement PM;
    GameObject player;
    Transform ExitDoor;
    PlayerOffScreen POS;
    private bool grounded, Enter;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        POS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerOffScreen>();
        ExitDoor = this.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        grounded = !PM.GetIsFalling();

        if (Input.GetButtonDown("DoorEnter") && Enter && grounded)
        {
            player.transform.position = new Vector3(ExitDoor.position.x , ExitDoor.position.y - ExitDoor.localScale.y *0.5f , ExitDoor.position.z);
            POS.summonCam();
        }

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            Enter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            Enter = false;
        }
    }
}