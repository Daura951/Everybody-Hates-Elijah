using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{

    private string gameObjectName;
    private CameraManager cameraManager;


    void Start()
    {
        gameObjectName = this.gameObject.name;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        cameraManager = GameObject.FindObjectOfType<CameraManager>();
        
        if(cameraManager == null)
        {
            Debug.Log("ERROR: No camera manager in scene");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if(gameObjectName.Contains("Wide Camera Trigger"))
            {
                if (cameraManager.currentOrthoSize != 8.0f)
                {
                    cameraManager.SetCameraSize(8.0f);
                }
                else cameraManager.SetCameraSize(5.0f);
            }
            else if (gameObjectName.Contains("Normal Camera Trigger"))
            {
                if (cameraManager.currentOrthoSize != 5.0f)
                {
                    cameraManager.SetCameraSize(5.0f);
                }
                else cameraManager.SetCameraSize(8.0f);
            }
            else
            {
                print(gameObjectName);
            }
        }
    }
}
