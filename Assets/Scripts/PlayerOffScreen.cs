using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffScreen : MonoBehaviour
{
    [Header("Camera info")]
    private Camera Cam;
    private Vector2 screenSize , N,S,E,W;
    public Vector3 offset;
    public float detectoff;

    [Header("Objects")]
    Health H;
    LedgeGrab lg;

    public bool loc;
    private float leftWall, rightWall, topWall, bottomWall;
    private bool update;
    private GameObject p;
    Rigidbody2D rb;

    public GameObject[] Detectors;
    Leave_Detection[] LD;

    bool fixcam;
    float YLoc;

    public float shakeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        p = this.transform.parent.gameObject;
        Cam = GetComponent<Camera>();
        rb = p.GetComponent<Rigidbody2D>();
        lg = p.GetComponent<LedgeGrab>();
        H = p.GetComponent<Health>();

        //Uses camera size to find the borders of the camera
        screenSize.x = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(Screen.width, 0f))) * 0.5f;
        screenSize.y = Vector2.Distance(Cam.ScreenToWorldPoint(new Vector2(0f, 0f)), Cam.ScreenToWorldPoint(new Vector2(0f, Screen.height))) * 0.5f;

        Detectors = new GameObject[transform.childCount];
        LD = new Leave_Detection[transform.childCount];
        for (int x = 0; x < Detectors.Length; x++)
        {
            Detectors[x] = transform.GetChild(x).gameObject;
            LD[x] = Detectors[x].GetComponent<Leave_Detection>();
        }

        StartCoroutine(Deatch());
        transform.position = p.transform.position + offset;

        summonCam();
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            if (!lg.action && !H.dead && !loc)
                WallDetection();

            rightWall = transform.position.x + screenSize.x;
            leftWall = transform.position.x - screenSize.x;
            topWall = transform.position.y + screenSize.y;
            bottomWall = transform.position.y - screenSize.y;

            if (((p.transform.position.x < leftWall) || (p.transform.position.x > rightWall) || (p.transform.position.y < bottomWall) || (p.transform.position.y > topWall)) && H.dead)
            {
                SpriteRenderer Sr = p.GetComponent<SpriteRenderer>();
                Sr.enabled = false;
            }
        }
    }

    IEnumerator Deatch()
    {
        yield return new WaitForSeconds(0f);
        this.transform.SetParent(null);
        DetectPos();
        update = true;
    }

    IEnumerator StartCheck()
    {
        yield return new WaitForSeconds(0.25f);
        if (LD[2].Detection() || LD[4].Detection())
        {
            fixcam = true;
            transform.position = p.transform.position + new Vector3(0f, screenSize.y - p.transform.localScale.y * 0.5f, 0f);
            YLoc = p.transform.position.y;
        }

    }

    IEnumerator shakeCamera()
    {
        Vector2 startPos = transform.position;
        float elapsedTime = 0.0f;

        print("SHAKE");
        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPos + Random.insideUnitCircle;
            yield return null;
        }

        transform.position = startPos;
    }

    public bool lockCam()
    {
        return loc;
    }

    public Vector3 Pos()
    {
        return transform.position;
    }

    private void WallDetection()
    {

        //Bottom Wall Fix
        if (fixcam && p.transform.position.y < transform.position.y - offset.y)
            transform.position = new Vector3(p.transform.position.x + offset.x, YLoc + screenSize.y - p.transform.localScale.y * 0.5f, p.transform.position.z + offset.z);
        else if (fixcam && p.transform.position.y >= transform.position.y - offset.y)
            fixcam = false;

        //Top Right Corner
        else if (LD[0].Detection() && LD[1].Detection() && p.transform.position.x > transform.position.x - offset.x && p.transform.position.y > transform.position.y - offset.y)
        transform.position = new Vector3(LD[0].pos.x, LD[1].pos.y, p.transform.position.z + offset.z);
        //Bottom Right Corner
        else if (LD[2].Detection() && LD[3].Detection() && p.transform.position.x > transform.position.x - offset.x && p.transform.position.y < transform.position.y - offset.y)
        transform.position = new Vector3(LD[2].pos.x, LD[3].pos.y, p.transform.position.z + offset.z);
        //Bottom Left Corner
        else if ( LD[4].Detection() && LD[5].Detection() && p.transform.position.x < transform.position.x - offset.x && p.transform.position.y < transform.position.y - offset.y)
        transform.position = new Vector3(LD[4].pos.x, LD[5].pos.y, p.transform.position.z + offset.z);
        //Top Left Corner
        else if (LD[6].Detection() && LD[7].Detection() && p.transform.position.x < transform.position.x - offset.x && p.transform.position.y > transform.position.y - offset.y)
        transform.position = new Vector3(LD[6].pos.x, LD[7].pos.y, p.transform.position.z + offset.z);
        //Right Wall
        else if (LD[1].Detection() && p.transform.position.x > transform.position.x - offset.x)
        transform.position = new Vector3(LD[1].pos.x, offset.y + p.transform.position.y, p.transform.position.z + offset.z);
        else if (LD[3].Detection() && p.transform.position.x > transform.position.x - offset.x)
        transform.position = new Vector3(LD[3].pos.x, offset.y + p.transform.position.y, p.transform.position.z + offset.z);
        //Left Wall
        else if (LD[7].Detection() && p.transform.position.x < transform.position.x - offset.x)
        transform.position = new Vector3(LD[7].pos.x, p.transform.position.y + offset.y, p.transform.position.z + offset.z);
        else if (LD[5].Detection() && p.transform.position.x < transform.position.x - offset.x)
        transform.position = new Vector3(LD[5].pos.x, p.transform.position.y + offset.y, p.transform.position.z + offset.z);
        //Top Wall
        else if (LD[6].Detection() && p.transform.position.y > transform.position.y - offset.y)
        transform.position = new Vector3(p.transform.position.x + offset.x, LD[6].pos.y, p.transform.position.z + offset.z);
        else if (LD[0].Detection() && p.transform.position.y > transform.position.y - offset.y)
        transform.position = new Vector3(p.transform.position.x + offset.x, LD[0].pos.y, p.transform.position.z + offset.z);
        //Bottom Wall
        else if (LD[2].Detection() && p.transform.position.y < transform.position.y - offset.y)
        transform.position = new Vector3(p.transform.position.x + offset.x, LD[2].pos.y, p.transform.position.z + offset.z);
        else if (LD[4].Detection() && p.transform.position.y < transform.position.y - offset.y)
        transform.position = new Vector3(p.transform.position.x + offset.x, LD[4].pos.y, p.transform.position.z + offset.z);
        else
        transform.position = p.transform.position + offset;
    }
    private void DetectPos()
    {
        float XScale = Detectors[0].transform.localScale.x;
        float YScale = Detectors[0].transform.localScale.y;
        //North
        N = new Vector2(transform.position.x,transform.position.y + screenSize.y + YScale * 0.5f);
        //East
        E = new Vector2(transform.position.x + screenSize.x + XScale * 0.5f, transform.position.y);
        //South
        S = new Vector2(transform.position.x,transform.position.y - screenSize.y - YScale * 0.5f);
        //West
        W = new Vector2(transform.position.x - screenSize.x -  XScale * 0.5f, transform.position.y);

        //Top.R
        Detectors[0].transform.position = new Vector2(E.x - XScale , N.y + detectoff);
        //Right.T
        Detectors[1].transform.position = new Vector2(E.x + detectoff, N.y - YScale);
        //Bottom.R
        Detectors[2].transform.position = new Vector2(E.x - XScale, S.y - detectoff);
        //Right.B
        Detectors[3].transform.position = new Vector2(E.x + detectoff, S.y + YScale);
        //Bottom.L
        Detectors[4].transform.position = new Vector2(W.x + XScale, S.y - detectoff);
        //Left.B
        Detectors[5].transform.position = new Vector2(W.x - detectoff, S.y + YScale);
        //Top.L
        Detectors[6].transform.position = new Vector2(W.x + XScale, N.y + detectoff);
        //Left.T
        Detectors[7].transform.position = new Vector2(W.x - detectoff, N.y - YScale);
    }

    public void summonCam()
    {
        StartCoroutine(StartCheck());
    }

    public void OffsetCam(Vector3 off)
    {
        transform.position = p.transform.position + offset + off;
    }
}
