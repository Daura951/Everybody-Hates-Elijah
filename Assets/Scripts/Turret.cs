using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;
    Bullet b;
    public float speed , range;
    private GameObject TurBullet;



    [Header("Turret")]
    public float bulletLaunchTime, DetectionTime;
    public float offsetX , offsetY , DetDis;
    public bool RayVisible;
    private Vector3 offset;
    private RaycastHit2D sensor;
    public  float timer = 0.0f, timeDetect = 0.0f;
    private bool Fire , On = true;

    ObjectHealth OH;

    // Start is called before the first frame update
    void Start()
    {
        OH = GetComponent<ObjectHealth>();
       
       bulletLaunchTime -= DetectionTime;

        if (transform.eulerAngles.y == 0)
            offset = new Vector3(transform.position.x - offsetX, transform.position.y + offsetY, transform.position.z);
        else
            offset = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        if (OH.GetHealth() <= 0)
            On = false;

        if (On)
        {
            if (transform.eulerAngles.y == 0)
                sensor = Physics2D.Raycast(offset, Vector3.left, DetDis);
            else
                sensor = Physics2D.Raycast(offset, Vector3.right, DetDis);

            if (RayVisible)
            {
                if (transform.eulerAngles.y == 0)
                    Debug.DrawRay(offset, Vector3.left * DetDis, Color.green);
                else
                    Debug.DrawRay(offset, Vector3.right * DetDis, Color.green);

            }

            if (sensor.collider != null && sensor.collider.CompareTag("Player"))
            {
                if (timeDetect >= DetectionTime)
                    timeDetect = DetectionTime;

                if (timeDetect == DetectionTime)
                {
                    Fire = true;

                }
                else timeDetect += Time.deltaTime;
            }
            else if (!Fire)
            {
                timeDetect = 0;
                timer = 0;
            }


            if (Fire)
            {
                if (timer >= bulletLaunchTime)
                    timer = bulletLaunchTime;

                if (timer == bulletLaunchTime)
                {
                    TurBullet = Instantiate(bullet, offset, Quaternion.Euler(0, transform.eulerAngles.y, 0));
                    b = TurBullet.GetComponent<Bullet>();
                    b.SetBull(speed, range, transform.eulerAngles.y);
                    Fire = false;
                    timer = 0;
                }
                else timer += Time.deltaTime;
            }

        }
    }
}