using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    private float up, down, left, right;
    private float Pup, Pdown, Pleft, Pright;
    public GameObject P, Arena, pCam, boxCam;
    bool active,finished;
    private ReSpawn R;

    int x = 0;
    GameObject[] BA;
    BattleArena[] BAS;

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        pCam = GameObject.FindGameObjectWithTag("MainCamera");
        boxCam = this.transform.GetChild(0).gameObject;
        R = P.GetComponent<ReSpawn>();
        Arena = this.transform.parent.gameObject;

        BAS = new BattleArena[transform.childCount-1];
        BA = new GameObject[transform.childCount-1];
        for (x=0; x < BA.Length; x++)
        {
            BA[x] = transform.GetChild(x+1).gameObject;
            BAS[x] = transform.GetChild(x+1).gameObject.GetComponent<BattleArena>();
        }
        x = 0;


        up = transform.position.y + (0.5f * transform.localScale.y);
        down = transform.position.y - (0.5f * transform.localScale.y) - .5f;
        left = transform.position.x - (0.5f * transform.localScale.x);
        right = transform.position.x + (0.5f * transform.localScale.x);
        
    }

    // Update is called once per frame
    void Update()
    {
        Pup = P.transform.position.y + (0.5f * P.transform.localScale.y);
        Pdown = P.transform.position.y - (0.5f * P.transform.localScale.y);
        Pleft = P.transform.position.x - (0.5f * P.transform.localScale.x);
        Pright = P.transform.position.x + (0.5f * P.transform.localScale.x);


        if(active)
        {
            BAS[x].enabled = true;
            if (BA[x].transform.childCount == 0)
            {
                if(x != BA.Length)
                {
                BAS[x].enabled = false;
                x++;
                }
                if (x == BA.Length)
                    finished = true;

            }
        }

        if (finished)
            Reset();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Pdown + P.transform.localScale.y > down && Pup - P.transform.localScale.y < up && Pleft + P.transform.localScale.x > left && Pright - P.transform.localScale.x < right && !active && !finished)
        {
            active = true;
            R.AssignArena(this);
            pCam.SetActive(false);
            boxCam.SetActive(true);
            Arena.transform.GetChild(0).gameObject.SetActive(true);
        }
    }


    public void Reset()
    {
        active = false;
        R.AssignArena(null);
        pCam.SetActive(true);
        boxCam.SetActive(false);
        Arena.transform.GetChild(0).gameObject.SetActive(false);
    }


}
