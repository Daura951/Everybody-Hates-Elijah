using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera camera;
    public float currentOrthoSize;

    public float zoomoutSpeed = 0.01f;

    private bool isTransitioning = false;
    private float targetCamSize;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        foreach (Transform child in player.transform)
        {
            if (child.name == "Camera")
            {
                camera = child.GetComponent<Camera>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTransitioning)
        {
            camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, targetCamSize, zoomoutSpeed);

            if(Mathf.Approximately(camera.orthographicSize, targetCamSize))
            {
                isTransitioning = false;
                currentOrthoSize = camera.orthographicSize;
            }
        }
    }

    public void SetCameraSize(float newCamSize)
    {
            targetCamSize = newCamSize;
            isTransitioning = true;
    }

    public bool getIsTransitioning()
    {
        return isTransitioning;
    }
}
