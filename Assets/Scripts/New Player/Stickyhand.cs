using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickyhand : MonoBehaviour
{
    [SerializeField]
    private GameObject stickyhand;
    private bool isStickyActive = false;

    void Start()
    {
        stickyhand.SetActive(isStickyActive);
    }

    public void ChangeStickyHandAnimation(string anim)
    {
        GetComponent<Animator>().Play(anim);
    }

    public void SetIsStickyActive(bool isStickyActive)
    {
        this.isStickyActive = isStickyActive;
        stickyhand.SetActive(isStickyActive);
    }
}
