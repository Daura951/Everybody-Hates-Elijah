using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArena : MonoBehaviour
{
    public GameObject[] Enemies;
    public float spawntime;
    private float timer = 0;
    bool start,finised;

    // Update is called once per frame
    void Update()
    {
        if(!finised)
        {
         if (spawntime <= timer)
         {
             spawntime = timer;
         }
         if (timer == spawntime)
         {
             start = true;
         }
         else spawntime -= Time.deltaTime;
        }




        if (start)
        {
         finised = true;
         start = false;
         foreach (GameObject g in Enemies)
         g.SetActive(true);

        }
    }
}
