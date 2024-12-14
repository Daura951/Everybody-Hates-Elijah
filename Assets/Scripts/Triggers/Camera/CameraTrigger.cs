using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraTrigger : MonoBehaviour
{

    protected CameraManager cameraManager;


    protected virtual void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        cameraManager = FindObjectOfType<CameraManager>();
        
        if(cameraManager == null)
        {
            Debug.Log("ERROR: No camera manager in scene");
        }
    }


    protected abstract void TriggerEffect();


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            TriggerEffect();
        }
    }
}
