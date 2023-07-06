using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotation : MonoBehaviour
{
    public float speed, RotateTarget;
    private float Rotate;
    private bool turn , Fall;
    private Vector3 spin = new Vector3(0f,0f,10f);
    
    // Start is called before the first frame update
    void Start()
    {
        turn = true;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate = transform.eulerAngles.z;

        if(Rotate > RotateTarget && Rotate < 180f && turn)
        {
        speed *= -1;
        turn = false;
        }

        if(Rotate < (360 - RotateTarget) && Rotate > 180f && !turn)
        {
        speed *= -1;
        turn = true;
        }

        transform.Rotate( spin * speed * Time.deltaTime);
    }
}
