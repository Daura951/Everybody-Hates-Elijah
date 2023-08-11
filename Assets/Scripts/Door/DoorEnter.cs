using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    PlayerMovement PM;
    GameObject player;
    Transform ExitDoor;
    private bool grounded, Enter;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        ExitDoor = this.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        grounded = !PM.GetIsFalling();

        if (Input.GetButtonDown("DoorEnter") && Enter && grounded)
        {
            player.transform.position = ExitDoor.position;
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