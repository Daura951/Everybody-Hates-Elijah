using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffScreen : MonoBehaviour
{
    [Header("Camera info")]
    private Camera Cam;
    private Vector3 CameraPosition;
    private Vector2 screenSize;
    public Vector3 offset;

    [Header("Objects")]
    Health H;

    public bool loc;
    private float leftWall, rightWall, topWall, bottomWall,minX, maxX, minY, maxY;
    private bool update;
    private GameObject p;
    private GameObject B;


    // Start is called before the first frame update
    void Start()
    {
        p = this.transform.parent.gameObject;
        StartCoroutine(Deatch());
        H = p.GetComponent<Health>();
        B = GameObject.FindGameObjectWithTag("Border");
        CameraPosition = p.transform.position + offset;

        maxX = B.transform.position.x + B.transform.localScale.x * 0.5f;
        minX = B.transform.position.x - B.transform.localScale.x * 0.5f;
        maxY = B.transform.position.y + B.transform.localScale.y * 0.5f;
        minY = B.transform.position.y - B.transform.localScale.y * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {  
       if(update)
        { 
        WallDetection();

        rightWall = CameraPosition.x + screenSize.x;
        leftWall = CameraPosition.x - screenSize.x;
        topWall = CameraPosition.y + screenSize.y;
        bottomWall = CameraPosition.y - screenSize.y;

        
            if (((p.transform.position.x < leftWall) || (p.transform.position.x > rightWall)||(p.transform.position.y < bottomWall)||(p.transform.position.y > topWall)) && H.dead)
            {
                SpriteRenderer Sr = p.GetComponent<SpriteRenderer>();
                Sr.enabled = false;
            }
       }
    }

    IEnumerator Deatch()
    {
        yield return new WaitForSeconds(0f);
        Cam = GetComponent<Camera>();
        this.transform.SetParent(null);

        //Uses camera size to find the borders of the camera
        screenSize.x = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f))) * 0.5f;
        screenSize.y = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(0f, Screen.height))) * 0.5f;

        update = true;
    }

    public bool lockCam()
    {
        return loc;
    }

    public Vector3 Pos()
    {
        return CameraPosition;
    }

    private void WallDetection()
    {
        //Top Right Corner
        if (p.transform.position.x + offset.x + screenSize.x >= maxX && p.transform.position.y + offset.y + screenSize.y >= maxY)
            CameraPosition = new Vector3(maxX - screenSize.x, maxY - screenSize.y, p.transform.position.z + offset.z);
        //Top Left Corner
        else if (p.transform.position.x + offset.x - screenSize.x <= minX && p.transform.position.y + offset.y + screenSize.y >= maxY)
            CameraPosition = new Vector3(minX + screenSize.x, maxY - screenSize.y, p.transform.position.z + offset.z);
        //Bottom Right Corner
        else if (p.transform.position.x + offset.x + screenSize.x >= maxX && p.transform.position.y + offset.y - screenSize.y <= minY)
            CameraPosition = new Vector3(maxX - screenSize.x, minY + screenSize.y, p.transform.position.z + offset.z);
        //Bottom Left Corner
        else if (p.transform.position.x + offset.x - screenSize.x <= minX && p.transform.position.y + offset.y - screenSize.y <= minY)
            CameraPosition = new Vector3(minX + screenSize.x, minY + screenSize.y, p.transform.position.z + offset.z);
        //Right Wall
        else if (p.transform.position.x + offset.x + screenSize.x >= maxX)
            CameraPosition = new Vector3(maxX - screenSize.x, p.transform.position.y + offset.y, p.transform.position.z + offset.z);
        //Left Wall
        else if (p.transform.position.x + offset.x - screenSize.x <= minX)
            CameraPosition = new Vector3(minX + screenSize.x, p.transform.position.y + offset.y, p.transform.position.z + offset.z);
        //Top Wall
        else if (p.transform.position.y + offset.y + screenSize.y >= maxY)
            CameraPosition = new Vector3(p.transform.position.x + offset.x, maxY - screenSize.y, p.transform.position.z + offset.z);
        //Bottom Wall
        else if (p.transform.position.y + offset.y - screenSize.y <= minY)
            CameraPosition = new Vector3(p.transform.position.x + offset.x, minY + screenSize.y, p.transform.position.z + offset.z);
        else
            CameraPosition = p.transform.position + offset;
    }
}
