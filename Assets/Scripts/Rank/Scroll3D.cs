using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll3D : MonoBehaviour
{

    public float scrollSpeed = 1.0f;
    public Vector3 scrollDirection = new Vector3(0.0f, 0.0f, 0.0f);
    public float xResetPos = -10.0f;

    private float startXPos;

    // Start is called before the first frame update
    void Start()
    {
        startXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(scrollDirection * scrollSpeed * Time.deltaTime);

        if(transform.position.x < xResetPos)
        {
            transform.position = new Vector3(startXPos, transform.position.y, transform.position.z);
        }
        
    }
}
