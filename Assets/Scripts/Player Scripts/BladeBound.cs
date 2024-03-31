using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeBound : MonoBehaviour
{

    [SerializeField] private Slider BBSlider;
    [SerializeField] private Slider freezeSlider;


    [SerializeField] private int maxHitsToBladeBound;
    private int curHits;

    [SerializeField] private int maxHitsToFreeze;
    private int curFreezeHits;

   [SerializeField] private float timeDelayWeight;

    private bool canFreeze;
    private bool isInBladeBound;

    public bool isFrozen = false;
    
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        BBSlider.value = 0;
        freezeSlider.value = 0;
        freezeSlider.gameObject.SetActive(false);
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(isInBladeBound)
        {
            canFreeze = true;
        }

        else
        {
            BBSlider.value = (float)curHits / maxHitsToBladeBound;
            if (BBSlider.value == 1)
            {
                player.GetComponent<PlayerAttack>().isBladeBound = true;
            }


            freezeSlider.value = 0;
            curFreezeHits = 0;
            freezeSlider.gameObject.SetActive(false);
        }


        if(canFreeze)
        {
            freezeSlider.gameObject.SetActive(true);
            freezeSlider.value = (float)curFreezeHits / maxHitsToFreeze;

            BBSlider.value -= Time.deltaTime / timeDelayWeight;

            if(curFreezeHits == maxHitsToBladeBound)
            {
                print("FREEZE!!!!!!!!");
                isFrozen = true;
            }
            if(BBSlider.value <= 0)
            {
                canFreeze = false;
                isInBladeBound = false;
                isFrozen = false;
                curHits = 0;
            }

        }
    }

    private void FixedUpdate()
    {

    }

    public int GetcurHits()
    {
        return curHits;
    }

    public void AddHit()
    {
        if (curHits < maxHitsToBladeBound)
        {
            curHits++;
        }
    }

    public void AddFreezeHit()
    {
        if (curFreezeHits < maxHitsToFreeze)
        {
            curFreezeHits++;
        }
    }

    public bool GetIsInBladeBound()
    {
        return isInBladeBound;
    }

    public void SetIsInBladeBound(bool newIsInBladeBound)
    {
         isInBladeBound = newIsInBladeBound;
    }
}
