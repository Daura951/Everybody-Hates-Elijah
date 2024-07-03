using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    private int videoIndex;
    private int sceneToGoTo;

    public void changeScene()
    {
        PlayerPrefs.SetInt("video", videoIndex);
        PlayerPrefs.SetInt("nextScene", sceneToGoTo);
        SceneManager.LoadScene(4);
    }

    public void SetVideoIndex(int videoIndex)
    {
        this.videoIndex = videoIndex;
    }

    public void SetSceneToGoTo(int sceneToGoTo)
    {
        this.sceneToGoTo = sceneToGoTo;
    }
}
