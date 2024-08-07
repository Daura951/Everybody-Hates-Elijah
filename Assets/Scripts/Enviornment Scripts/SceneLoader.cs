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

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Pdown > down && Pup < up && Pleft > left && Pright < right)
        {
            LoaderCallback.targetScene = scene;
            SceneManager.LoadScene(1);
        }
    }
}
