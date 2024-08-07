using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraTrigger : CameraTrigger
{
    public float rotationAngle;

    protected override void Start()
    {
        base.Start();
    }

    protected override void TriggerEffect()
    {
        cameraManager.SetCameraRotation(rotationAngle);
    }
}
