using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    PlayerMovement PM;
    GameObject player;
    Transform EnterDoor;
    PlayerOffScreen POS;
    private bool grounded, Enter;
    public float wait;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<PlayerMovement>();
        POS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerOffScreen>();
        EnterDoor = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = !PM.GetIsFalling();

        if (Input.GetButtonDown("DoorEnter") && Enter && grounded)
        {
            StartCoroutine(teleport());
        }

    }

    IEnumerator teleport()
    {
        PauseSystem.ChangeGameState();
        player.SetActive(false);
        yield return new WaitForSecondsRealtime(wait);
        PauseSystem.ChangeGameState();
        player.transform.position = new Vector3(EnterDoor.position.x, EnterDoor.position.y - EnterDoor.localScale.y * 0.5f, EnterDoor.position.z);
        player.SetActive(true);
        POS.summonCam();

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