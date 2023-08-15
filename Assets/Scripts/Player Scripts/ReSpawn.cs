using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    Health H;
    PlayerMovement PM;
    Vector2 ReSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
      H = GetComponent<Health>();
      PM = GetComponent<PlayerMovement>();
      ReSpawnPoint = new Vector2(PM.transform.position.x , PM.transform.position.y); 
    }

    // Update is called once per frame
    void Update()
    {
        if( H.GetHealth() == 0)
        {
        PM.transform.position = ReSpawnPoint;
        H.ReHeal();
        }
    }

    public void NewCheckPoint(Vector2 c)
    {
        ReSpawnPoint = c ;
    }
}
