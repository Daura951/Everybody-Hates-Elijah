using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float time;
    private bool start = false;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        if(start)
        {
            if(time > 0)
            {
            float seconds = Mathf.FloorToInt(time % 60);
            Debug.Log(seconds);

            time-= Time.deltaTime;
            }
            else
            time = 0;

            if(time == 0)
                fall(rb);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            start = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            start = false;
        }
    }


    private void fall (Rigidbody2D rb)
    {
       rb.bodyType = RigidbodyType2D.Dynamic;
    }
}