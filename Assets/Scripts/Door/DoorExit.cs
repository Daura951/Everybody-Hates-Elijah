using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    PlayerMovement PM;
    GameObject player;
    Transform EnterDoor;
    private bool grounded, Enter;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        EnterDoor = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = !PM.GetIsFalling();

        if (Input.GetButtonDown("DoorEnter") && Enter && grounded)
        {
            player.transform.position = EnterDoor.position;
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