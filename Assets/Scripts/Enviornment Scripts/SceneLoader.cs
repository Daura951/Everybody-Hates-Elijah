using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private float up, down, left, right;
    private float Pup, Pdown, Pleft, Pright;
    public int scene;
    private GameObject P;
    private bool teleport = true, check = false;


    // Start is called before the first frame update
    void Start()
    {
        up = transform.position.y + (0.5f * transform.localScale.y);
        down = transform.position.y - (0.5f * transform.localScale.y) - .5f;
        left = transform.position.x - (0.5f * transform.localScale.x);
        right = transform.position.x + (0.5f * transform.localScale.x);

        P = GameObject.Find("Player");
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
        if (Pdown > down && Pup < up && Pleft > left && Pright < right && teleport && check)
        {
            SceneManager.LoadScene(scene);
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
