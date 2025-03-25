using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private Player player;
    private StunController stun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CutsceneTrigger")
        {
            collision.gameObject.GetComponent<CutsceneTrigger>().isTriggered = true;
        }
    }
}
