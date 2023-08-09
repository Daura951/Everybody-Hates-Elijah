using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyHand : MonoBehaviour
{
    public float maxCountDown;
    private float countDown;
    public float speed;
    public float editedSpeed;
    private float endPos;
    public bool goBack = false;
    public PlayerMovement playerMove;
    public PlayerAttack playerAttack;
    public LineRenderer render;
    bool playedSuccess = false;


    private void OnEnable()
    {
        countDown = maxCountDown;
        endPos = transform.position.x;
        goBack = false;

        if(GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
        playerAttack.isSticked = false;
        playedSuccess = false;
        editedSpeed = speed;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        countDown -= Time.deltaTime;
        bool isPlayerLeft = playerMove.GetIsLeft();


        if (render != null)
        {
            render.SetPosition(0, new Vector3(0, 0, 0));
            render.SetPosition(1, new Vector3( isPlayerLeft ? -(transform.position.x - playerMove.transform.position.x) : transform.position.x - playerMove.transform.position.x, 0, 0));
        }



        if (countDown > 0)
        {
            transform.position += new Vector3(isPlayerLeft ? -1 :  1, 0, 0) * Time.deltaTime * editedSpeed;
            editedSpeed += .1f;
        }

        else if (goBack || countDown < maxCountDown+.05)
        {
            if(GetComponent<CircleCollider2D>() != null && !playedSuccess)
            { 
                if(playerAttack.isSticked)
                {
                    print("Success");
                    if(!playedSuccess)
                    { 
                        playerAttack.anim.Play("Neutral B Success"); 
                    }
                    
                    playedSuccess = true;
                }
                else
                {
                    playerAttack.anim.Play("Neutral B Fail");
                    if (!playedSuccess)
                    { 
                        print("Fail"); 
                    }
                    playedSuccess = true;
                }
            }

            transform.position += new Vector3(isPlayerLeft ? 1 : -1, 0, 0) * Time.deltaTime * (playerAttack.isSticked ? speed : editedSpeed/2);

            if(!isPlayerLeft &&  transform.position.x < endPos || isPlayerLeft && transform.position.x > endPos)
            {
                gameObject.SetActive(false);
            }
        }
      
    }

    private void OnDisable()
    {
        render.SetPosition(1, new Vector3(0, 0, 0));
    }
}

