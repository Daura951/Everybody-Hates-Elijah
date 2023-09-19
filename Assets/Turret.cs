using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public GameObject bullet;

    public float bulletLaunchTime = 2.0f;
    public Vector3 launchPos;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= bulletLaunchTime)
        {
            Instantiate(bullet, launchPos, Quaternion.identity);
            timer = 0;
        }
        else timer += Time.deltaTime;
    }
}
