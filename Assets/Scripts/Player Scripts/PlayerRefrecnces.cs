using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRefrecnces : MonoBehaviour
{

    public static PlayerRefrecnces instance;


    public PlayerAttack Attack;
    public PlayerMovement Move;
    public PlayerTaunt Taunt;
    public Stun Stun;
    public Rigidbody2D RB;
    public Animator anim;


    void Start()
    { 
          instance = this;
        if(SceneManager.GetActiveScene().buildIndex != 0)
        PlayerPrefs.SetInt("loadGameIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("playerPosX", GameObject.FindGameObjectWithTag("Player").transform.position.x);
        PlayerPrefs.SetFloat("playerPosY", GameObject.FindGameObjectWithTag("Player").transform.position.y);
    }


    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("goToStartMenu", 1);
        PlayerPrefs.SetFloat("playerPosX", GameObject.FindGameObjectWithTag("Player").transform.position.x);
        PlayerPrefs.SetFloat("playerPosY", GameObject.FindGameObjectWithTag("Player").transform.position.y);
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}


