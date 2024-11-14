using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    private Transform player;
    public Vector3 offset;

    private bool isLocked = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (!isLocked)
        {
            transform.position = new Vector3(player.position.x, player.position.y, 0.0f) + offset;
        }
    }

    public void SetIsLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }
}
