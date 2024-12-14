using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockTrigger : CameraTrigger
{
    [SerializeField]
    private GameObject walls;

    [SerializeField]
    private Vector3 lockedZoneCamPos;

    private WaveSpawner spawner;

    protected override void Start()
    {
        spawner = GetComponent<WaveSpawner>();
        walls.SetActive(false);
        base.Start();
        spawner.OnWavesEnd.AddListener(EndEffect);
    }

    protected override void TriggerEffect()
    {
        walls.SetActive(true);
        cameraManager.SetCameraLock(true);
        cameraManager.SetCameraPosition(lockedZoneCamPos);
    }

    public void EndEffect()
    {
        walls.SetActive(false);
        cameraManager.SetCameraLock(false);
        Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TriggerEffect();
            spawner.StartWaves();
        }
    }
}
