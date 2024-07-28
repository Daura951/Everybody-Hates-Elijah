using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GeneralSceneFader fader;

    public Animator elijahAnim;
    public GameObject player, menuCam, mainMenu, playerUI, bubble;

    public void StartNewGame()
    {
        mainMenu.SetActive(false);
        bubble.SetActive(false);
        menuCam.SetActive(false);
        elijahAnim.gameObject.SetActive(false);
        player.SetActive(true);
        playerUI.SetActive(false);

        //fader.SetSceneToGoTo(5);
        //fader.GetComponent<Animator>().Play("Fade");
    }

    public void LoadGame()
    {
        bubble.SetActive(false);
        elijahAnim.SetInteger("nextAnim", 3);
        LoaderCallback.targetScene = PlayerPrefs.GetInt("loadGameIndex");
        fader.SetSceneToGoTo(7);
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
