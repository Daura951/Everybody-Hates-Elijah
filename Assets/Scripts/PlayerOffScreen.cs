using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffScreen : MonoBehaviour
{
    private Vector3 CameraPosition;
    private Vector2 screenSize;
    private Vector3 topCollider;
    private Vector3 bottomCollider;
    private Vector3 rightCollider;
    private Vector3 leftCollider;
    private bool update;
    private Camera Cam;

    public float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Deatch());
    }

    // Update is called once per frame
    void Update()
    {  
       if(update)
       {

        CameraPosition = this.transform.position;
        screenSize.x = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f))) * 0.5f;
        screenSize.y = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(0f, Screen.height))) * 0.5f;
        
        rightCollider= new Vector3(CameraPosition.x + screenSize.x, CameraPosition.y, 0f);
        leftCollider= new Vector3(CameraPosition.x - screenSize.x, CameraPosition.y, 0f);
        topCollider = new Vector3(CameraPosition.x, CameraPosition.y + screenSize.y, 0f);
        bottomCollider= new Vector3(CameraPosition.x, CameraPosition.y - screenSize.y, 0f);

        Debug.DrawRay(CameraPosition, rightCollider- CameraPosition, Color.green);
        Debug.DrawRay(CameraPosition, leftCollider- CameraPosition, Color.red);
        Debug.DrawRay(CameraPosition, topCollider- CameraPosition, Color.blue);
        Debug.DrawRay(CameraPosition, bottomCollider- CameraPosition, Color.yellow);


        if (transform.position.x < leftCollider.x)
            Debug.Log("BYEEEEEEE");
        if (transform.position.x > rightCollider.x)
            Debug.Log("BYEEEEEEE");
        if (transform.position.y < bottomCollider.y)
            Debug.Log("BYEEEEEEE");
        if (transform.position.y > topCollider.y)
            Debug.Log("BYEEEEEEE");
       }
    }

    IEnumerator Deatch()
    {
        yield return new WaitForSeconds(time);
        this.transform.SetParent(null);
         Cam = GetComponent<Camera>();
        update = true;
    }
}
