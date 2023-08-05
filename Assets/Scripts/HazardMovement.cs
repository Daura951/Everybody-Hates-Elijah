using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMovement : MonoBehaviour
{
    public float speed = 1f;
    public bool change = false;
    public Vector3 target;
    private Vector3 position;

    void Start()
    {
        position = gameObject.transform.position;

        
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if(!change)
        {
        transform.position = Vector2.MoveTowards(transform.position, target, step);
            if(transform.position == target)
                change = !change;
        }


        if(change)
        {
        transform.position = Vector2.MoveTowards(transform.position, position, step);
            if(transform.position == position)
                change = !change;
        }
    }
       
}

