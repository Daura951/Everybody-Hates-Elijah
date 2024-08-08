using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoCutsceneFader : Fader
{
    private int videoIndex;
    public override void OnFade()
    {
        PlayerPrefs.SetInt("video", videoIndex);
        PlayerPrefs.SetInt("nextScene", sceneToGoTo);
        SceneManager.LoadScene(3);
    }

    public void SetVideoIndex(int videoIndex)
    {
        this.videoIndex = videoIndex;
    }
}
