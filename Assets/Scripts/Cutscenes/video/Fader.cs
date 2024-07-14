using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    private int videoIndex;
    private int sceneToGoTo;
    private bool isCutscene;

    public void changeScene()
    {
        if (isCutscene)
        {
            PlayerPrefs.SetInt("video", videoIndex);
            PlayerPrefs.SetInt("nextScene", sceneToGoTo);
            SceneManager.LoadScene(4);
        }
        else
        {
            SceneManager.LoadScene(sceneToGoTo);
        }
    }

    public void SetVideoIndex(int videoIndex)
    {
        this.videoIndex = videoIndex;
    }

    public void SetSceneToGoTo(int sceneToGoTo)
    {
        this.sceneToGoTo = sceneToGoTo;
    }

    public void SetIsCutscene(bool isCutscene)
    {
        this.isCutscene = isCutscene;
    }
}
