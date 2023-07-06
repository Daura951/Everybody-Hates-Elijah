using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject_Timer : MonoBehaviour
{
    public float build;
    private float clock;
    private bool spawn = false;
    [SerializeField] private GameObject item;

    void Start()
    {
        clock = build;
    }

    void Update()
    {
         if(spawn)
         {
          if(clock > 0)
           {
           float seconds = Mathf.FloorToInt(clock % 60);
           Debug.Log(seconds);
           clock-= Time.deltaTime;
           }
          else
           clock = 0;
          if(clock == 0)
           summon();
         }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            spawn = true;
        }
    }


    private void summon()
    {
       Instantiate(item);
       clock = build;
       spawn = false;
    }
}
