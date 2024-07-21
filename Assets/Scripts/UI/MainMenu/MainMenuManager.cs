using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GeneralSceneFader fader;

    public void StartNewGame()
    {
        fader.SetSceneToGoTo(5);
        fader.GetComponent<Animator>().Play("Fade");
    }

    public void LoadGame()
    {
        fader.SetSceneToGoTo(PlayerPrefs.GetInt("loadGameIndex"));
        fader.GetComponent<Animator>().Play("Fade");
    }

    public void LoadOptions()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
