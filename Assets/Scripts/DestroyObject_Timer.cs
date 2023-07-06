using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject_Timer : MonoBehaviour
{
    public float destroy;
    private bool del = false;

    void Update()
    {
         if(del)
         {
          if(destroy > 0)
           {
           float seconds = Mathf.FloorToInt(destroy % 60);
           Debug.Log(seconds);
           destroy-= Time.deltaTime;
           }
          else
           destroy = 0;
          if(destroy == 0)
           Delete();
         }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            del = true;
        }
    }


    private void Delete()
    {
       Destroy(this.gameObject);
    }
}
