using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockTrigger : CameraTrigger
{
    [SerializeField]
    private GameObject walls;

    protected override void Start()
    {
        walls.SetActive(false);
        base.Start();
    }

    protected override void TriggerEffect()
    {
        walls.SetActive(true);
        cameraManager.SetCameraLock(true);
    }

    public void EndEffect()
    {
        walls.SetActive(false);
        cameraManager.SetCameraLock(false);
    }
}
