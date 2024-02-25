using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffScreen : MonoBehaviour
{
    public Vector3 CameraPosition;
    public Vector2 screenSize;
    private Transform topCollider;
    private Transform bottomCollider;
    private Transform rightCollider;
    private Transform leftCollider;
    public Vector3 center = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

        topCollider = new GameObject().transform;
        bottomCollider = new GameObject().transform;
        rightCollider = new GameObject().transform;
        leftCollider = new GameObject().transform;

        topCollider.name = "TopCollider";
        bottomCollider.name = "BottomCollider";
        rightCollider.name = "RightCollider";
        leftCollider.name = "LeftCollider";

    }

    // Update is called once per frame
    void Update()
    {   CameraPosition = Camera.main.transform.position;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)), Camera.main.ScreenToWorldPoint(new Vector2(0f, Screen.height))) * 0.5f;
        
        rightCollider.position = new Vector3(CameraPosition.x + screenSize.x + (rightCollider.localScale.x * 0.5f), CameraPosition.y, 0f);

        leftCollider.position = new Vector3(CameraPosition.x - screenSize.x - (leftCollider.localScale.x * 0.5f), CameraPosition.y, 0f);

        topCollider.position = new Vector3(CameraPosition.x, CameraPosition.y + screenSize.y + (topCollider.localScale.y * 0.5f), 0f);

        bottomCollider.position = new Vector3(CameraPosition.x, CameraPosition.y - screenSize.y - (bottomCollider.localScale.y * 0.5f), 0f);

        Debug.DrawRay(center, rightCollider.position, Color.green);
        Debug.DrawRay(center, leftCollider.position, Color.red);
        Debug.DrawRay(center, topCollider.position, Color.blue);
        Debug.DrawRay(center, bottomCollider.position, Color.yellow);


        if (transform.position.x < leftCollider.position.x)
            Debug.Log("BYEEEEEEE");
        if (transform.position.x > rightCollider.position.x)
            Debug.Log("BYEEEEEEE");
        if (transform.position.y < bottomCollider.position.y)
            Debug.Log("BYEEEEEEE");
        if (transform.position.y > topCollider.position.y)
            Debug.Log("BYEEEEEEE");



    }
}
