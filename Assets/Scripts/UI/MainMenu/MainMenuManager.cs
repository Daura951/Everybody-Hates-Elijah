using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Fader fader;

    public Animator elijahAnim;
    public GameObject player, menuCam, mainMenu, playerUI, bubble;

    private void Awake()
    {

        //foreach(GameObject vct in GameObject.FindGameObjectsWithTag("VideoCutsceneTrigger"))
        //{
        //    vct.GetComponent<VideoCutsceneTrigger>().CheckIfShouldBeActive();
        //}
        
    }

    private void Start()
    {
        //PlayerPrefs.SetFloat("playerPosX", 14.1f);
        print(PlayerPrefs.GetFloat("playerPosX") + " " + PlayerPrefs.GetFloat("playerPosY"));

        if(PlayerPrefs.GetInt("goToStartMenu") == 0)
        {
            print("I am not going to main menu");
            loadMenuLevel();
        }
        print("GoToStartMenu: " + PlayerPrefs.GetInt("goToStartMenu"));
    }

    private void loadMenuLevel()
    {
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("playerPosX"), PlayerPrefs.GetFloat("playerPosY"), player.transform.position.z);
        DestroyMenuObjects();
    }

    public void StartNewGame()
    {
        DestroyMenuObjects();
        LoaderCallback.targetScene = 5;

        for(int i =0; i < GlobalGameConstants.AMT_OF_CUTSCENES; i++)
        {
            PlayerPrefs.SetInt("cutscene_" + i, 0);
        }
    }

    public void LoadGame()
    {
        bubble.SetActive(false);
        elijahAnim.SetInteger("nextAnim", 3);
        LoaderCallback.targetScene = PlayerPrefs.GetInt("loadGameIndex");
        fader.SetSceneToGoTo(2);
        fader.GetComponent<Animator>().Play("Fade");
    }

    public void LoadOptions()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void DestroyMenuObjects()
    {
        mainMenu.SetActive(false);
        bubble.SetActive(false);
        menuCam.SetActive(false);
        elijahAnim.gameObject.SetActive(false);
        player.SetActive(true);

        for (int i = 0; i < playerUI.transform.childCount; i++)
        {
            if (!playerUI.transform.GetChild(i).name.Contains("Fader"))
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
