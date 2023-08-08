using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Target : MonoBehaviour
{
    private GameObject player;
    PlayerMovement PM;
    private Vector3 Scale;

    public float speed , MaxDistance;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
      player = GameObject.FindGameObjectWithTag("Player");
      PM = player.GetComponent<PlayerMovement>();
      Scale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position , player.transform.position);
        Vector2 dir = player.transform.position - transform.position;

        if(distance<=MaxDistance && (this.transform.position.x != player.transform.position.x))
        transform.position = Vector2.MoveTowards(this.transform.position , player.transform.position , speed * Time.deltaTime );

        if(dir.x < 0 )
        this.transform.localScale = new Vector3(Scale.x , Scale.y , Scale.z);

        else if(dir.x > 0 )
        this.transform.localScale = new Vector3(-Scale.x , Scale.y , Scale.z);
    }
}
