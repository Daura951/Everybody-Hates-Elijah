using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera camera;

    public float zoomoutSpeed = 0.1f;
    public float rotSpeed = 0.1f;

    private bool isTransitioning = false;
    private bool isRotating = false;

    private float targetCamSize;
    private float targetYAngle;

    private Stack<float> angleStack = new Stack<float>();
    private Stack<float> FOVStack = new Stack<float>();

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

        camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTransitioning)
        {
            camera.fieldOfView = Mathf.MoveTowards(camera.fieldOfView, targetCamSize, zoomoutSpeed);

            if(Mathf.Approximately(camera.fieldOfView, targetCamSize))
            {
                isTransitioning = false;
            }
        }

        else if(isRotating)
        {
            float newAngle = Mathf.MoveTowardsAngle(camera.transform.eulerAngles.y, targetYAngle, rotSpeed);
            camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, newAngle, camera.transform.eulerAngles.z);

            if(Mathf.Approximately(camera.transform.rotation.y, targetYAngle))
            {
                isRotating = false;
            }

        }
    }

    public void SetCameraSize(float newCamSize)
    { 

        if(FOVStack.Count == 0 || !Mathf.Approximately(targetCamSize, newCamSize))
        {
            FOVStack.Push(camera.fieldOfView);
        }

        targetCamSize = DetermineCamEffect(camera.fieldOfView, newCamSize, FOVStack.Count) ? FOVStack.Pop() : newCamSize;

        isTransitioning = true;
    }

    public void SetCameraRotation(float newAngle)
    {
        //If we are at length 0 in stack, or if the target angle does not equal new angle, then add to the stack of angles
        if (angleStack.Count == 0 || !Mathf.Approximately(targetYAngle, newAngle))
        {
            angleStack.Push(camera.transform.eulerAngles.y);
        }

        targetYAngle = DetermineCamEffect(camera.transform.eulerAngles.y, newAngle, angleStack.Count) ? angleStack.Pop() : newAngle;
        isRotating = true;
    }

    public void SetCameraLock(bool isLocked)
    {
        camera.GetComponent<TempCameraFollow>().SetIsLocked(isLocked);
    }


    //True if we use stack value, flase if we use new value
    //use booleans becuase of pointers. Passing by reference won't work and I don't wanna make a wrapper class for a single data structure
    private bool DetermineCamEffect(float curVal, float newval, int stackLen)
    {
        //if the current value of rotation equals the value in the trigger, if means that we wanna rotate to previous angle
        if(stackLen > 0 && Mathf.Approximately(curVal, newval))
        {
            return true;        
        }


        //else this is a new trigger so rotate to its value
        else
        {
            return false;
        }
    }

}
