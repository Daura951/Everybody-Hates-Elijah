using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{

    [SerializeField]
    private RawImage image;

    [SerializeField]
    private float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(scrollSpeed, 0) * Time.deltaTime, image.uvRect.size);
    }
}
