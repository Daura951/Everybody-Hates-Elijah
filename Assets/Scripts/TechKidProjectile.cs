using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechKidProjectile : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;
    Bullet b;
    public float speed , range;
    private GameObject TurBullet;



    [Header("Tech Kid Projectile")]
    public float bulletLaunchTime, DetectionTime;
    public float offsetX , offsetY , DetDis;
    public bool RayVisible;
    private Vector3 offset;
    private RaycastHit2D sensor;
    private Animator anim;
    private float[] stats;

    public  float timer = 0.0f, timeDetect = 0.0f;
    private bool Fire , On = true;
    public Transform sniper;

    public Transform bulletPoint;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
       bulletLaunchTime -= DetectionTime;

        if (transform.eulerAngles.y == 0)
            offset = new Vector3(transform.position.x - offsetX, transform.position.y + offsetY, transform.position.z);
        else
            offset = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (GetComponent<EnemyHealth>().health <= 0)
            {
                anim.Play("Dead");
                isDead = true;
            }

            if (On)
            {
                if (transform.eulerAngles.y == 0)
                    sensor = Physics2D.CircleCast(offset, 2, Vector3.left);
                else
                    sensor = Physics2D.CircleCast(offset, 2, Vector3.right);


                if (sensor.collider != null && sensor.collider.CompareTag("Player"))
                {
                    Vector3 target = sensor.collider.gameObject.GetComponent<Transform>().position;
                    target.z = 0f;

                    Vector3 objectPos = sniper.position;
                    target.x = target.x - objectPos.x;
                    target.y = target.y - objectPos.y;

                    float rotationAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                    sniper.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));

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
                        TurBullet = Instantiate(bullet, bulletPoint.position, Quaternion.Euler(sniper.eulerAngles.x, sniper.eulerAngles.y, sniper.eulerAngles.z));
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

    public void Dead()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(offset, 2);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox" && !(collision.gameObject.name == "StickyHandHitbox") && collision.gameObject.name != "Grab Hiitbox")
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().GetCurrentStats();
            anim.Play("Hurt");
            timeDetect = 0;
            timer = 0;
            GetComponent<EnemyHealth>().TakeDamage(stats[0]);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().Combo();

        }
    }
}