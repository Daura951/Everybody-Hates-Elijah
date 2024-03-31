using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave_Detection : MonoBehaviour
{
    public bool outside;
    public GameObject parent;
    public Vector2 pos;
    public void Start()
    {
        parent = this.transform.parent.gameObject;
    }

    public void Update()
    {
        if(outside && (pos.x != parent.transform.position.x || pos.y != parent.transform.position.y))
            pos = parent.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            pos = new Vector2();
            outside = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            pos = parent.transform.position;
            outside = true;
        }
    }

    public bool Detection()
    {
        return outside;
    }
}
