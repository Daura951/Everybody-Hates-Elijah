using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidenCameraTrigger : CameraTrigger
{

    public float fovZoom;
    protected override void Start()
    {
        base.Start();
    }

    protected override void TriggerEffect()
    {
        cameraManager.SetCameraSize(fovZoom);
    }
}
