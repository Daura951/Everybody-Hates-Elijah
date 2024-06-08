using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private SpriteRenderer sr;
    ReSpawn Respawn;

    private bool CheckPoint = false;
    private Vector3 pos;
    public Sprite s1;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        pos = gameObject.transform.position;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!CheckPoint && col.gameObject.GetComponent<ReSpawn>())
        {
            CheckPoint = true;
            Respawn = col.gameObject.GetComponent<ReSpawn>();
            Respawn.NewCheckPoint(pos);
            if(s1 != null)
            sr.sprite = s1;
        }
    }
}
