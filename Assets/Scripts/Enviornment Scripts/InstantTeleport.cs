using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTeleport : MonoBehaviour
{
    private float up, down, left, right;
    private float Pup, Pdown, Pleft, Pright;
    public Vector3 spot2;
    private GameObject P;
    private bool teleport = true ,check = false;
    void Start()
    {
     
        up = transform.position.y + (0.5f * transform.localScale.y);
        down = transform.position.y - (0.5f * transform.localScale.y)-.5f;
        left = transform.position.x - (0.5f * transform.localScale.x);
        right = transform.position.x + (0.5f * transform.localScale.x);

         P = GameObject.Find("Player");
        if (!this.name.Contains("(Clone)"))
            Instantiate(this, spot2, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Pup = P.transform.position.y + (0.5f * P.transform.localScale.y);
        Pdown = P.transform.position.y - (0.5f * P.transform.localScale.y);
        Pleft = P.transform.position.x - (0.5f * P.transform.localScale.x);
        Pright = P.transform.position.x + (0.5f * P.transform.localScale.x);

        if (Pup < down || Pdown > up || Pright < left || Pleft > right)
        {
            teleport = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        check = false;
        StartCoroutine(TeleportWait());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Pdown > down && Pup < up && Pleft > left && Pright < right && teleport && check)
        {
            if (!this.name.Contains("(Clone)"))
            {
                GameObject d2 = GameObject.Find(this.name + "(Clone)");
                P.transform.position = d2.transform.position;
            }
            else if (this.name.Contains("(Clone)"))
            {
                GameObject d1 = GameObject.Find(this.name.Substring(0,this.name.Length-7));
                P.transform.position = d1.transform.position;
            }
        }
    }


    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(.0125f);
        if (Pdown > down && Pup < up && Pleft > left && Pright < right)
        {
            teleport = false;
        }
            check = true;
    }
}
