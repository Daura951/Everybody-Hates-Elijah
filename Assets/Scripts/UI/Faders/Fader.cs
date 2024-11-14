using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{

    private int sceneToGoTo;
    private int videoIndex;

    private Vector3 positionToGoto;

    public Transform playerTf;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void OnFadeScene()
    {
        LoaderCallback.targetScene = sceneToGoTo;
        SceneManager.LoadScene(2);
    }

    public void OnFadeVideo()
    {
        PlayerPrefs.SetInt("video", videoIndex);
        PlayerPrefs.SetInt("nextScene", sceneToGoTo);
        SceneManager.LoadScene(3);
    }

    public void OnFadeTeleport()
    {
        playerTf.position = positionToGoto;
    }

    public void SetSceneToGoTo(int sceneToGoTo)
    {
        this.sceneToGoTo = sceneToGoTo;
    }

    public void SetVideoIndex(int videoIndex)
    {
        this.videoIndex = videoIndex;
    }

    public void setPositionToGoTo(Vector3 positionToGoto)
    {
        this.positionToGoto = positionToGoto;
    }

    public void Fade(string fadeType)
    {
        if(fadeType == "video")
        {
            anim.Play("VideoFade");
        }
        else if(fadeType == "scene")
        {
            anim.Play("SceneChangeFade");
        }
        else if(fadeType == "position")
        {
            anim.Play("TeleportTransition");
        }
    }

}
