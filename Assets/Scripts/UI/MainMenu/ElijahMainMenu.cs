using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElijahMainMenu : MonoBehaviour
{

    public Animator elijahAnim;
    public GameObject thoughtBubble;
    public int poseNumber;

    public void ChangePose()
    {
        poseNumber = Random.Range(0, 2);
        elijahAnim.SetInteger("nextAnim", poseNumber);
    }

    public void ChangeThoughtBubbleVisibility()
    {
        thoughtBubble.SetActive(!thoughtBubble.activeSelf);
    }
}
