using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyHand : MonoBehaviour
{
    private PlayerRefrecnces PR;

    public float maxCountDown;
    private float countDown;
    public float speed;
    public float editedSpeed;
    private float endPos;
    public bool goBack = false;

    public LineRenderer render;
    bool playedSuccess = false;
    public AudioSource AS;


    private void OnEnable()
    {
        countDown = maxCountDown;
        endPos = transform.position.x;
        goBack = false;

        if(GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
        
        playedSuccess = false;
        editedSpeed = speed;

    }

    void Start()
    {
        PR = PlayerRefrecnces.instance; //player references is a singleton, so we gotta make sure we use the instance
        PR.Attack.isSticked = false;
    }

    // Update is called once per frame
    void Update()
    {

        countDown -= Time.deltaTime;
        bool isPlayerLeft = PR.Move.GetIsLeft();


        if (render != null)
        {
            render.SetPosition(0, new Vector3(0, 0, 0));
            render.SetPosition(1, new Vector3( isPlayerLeft ? -(transform.position.x - PR.Move.transform.position.x) : transform.position.x - PR.Move.transform.position.x, 0, 0));
        }



        if (countDown > 0)
        {
            transform.position += new Vector3(isPlayerLeft ? -1 :  1, 0, 1) * Time.deltaTime * editedSpeed;
            editedSpeed += .1f;
        }

        else if (goBack || countDown < maxCountDown+.05)
        {
            if(GetComponent<CircleCollider2D>() != null && !playedSuccess)
            { 
                if(PR.Attack.isSticked)
                {
                    print("Success");
                    if(!playedSuccess)
                    { 
                        PR.anim.Play("Neutral B Success"); 
                    }
                    
                    playedSuccess = true;
                }
                else
                {
                    PR.anim.Play("Neutral B Fail");
                    if (!playedSuccess)
                    { 
                        print("Fail"); 
                    }
                    playedSuccess = true;
                }
            }

            transform.position += new Vector3(isPlayerLeft ? 1 : -1, 0, 1) * Time.deltaTime * (PR.Attack.isSticked ? speed : editedSpeed/2);

            if (gameObject != null)
            {
                if (!isPlayerLeft && transform.position.x < endPos || isPlayerLeft && transform.position.x > endPos)
                {
                    gameObject.SetActive(false);
                }
            }
        }
      
    }

    private void OnDisable()
    {
        render.SetPosition(1, new Vector3(0, 0, 0));
    }

    public void A12345()
    {
        if (!AS.isPlaying)
            AS.Play();
    }
}

