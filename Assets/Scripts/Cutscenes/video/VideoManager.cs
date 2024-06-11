using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    // Start is called before the first frame update

    public VideoClip[] videos;
    public VideoPlayer player;
    void Start()
    {
        player.loopPointReached += OnVideoEnd; 
        player.clip = videos[PlayerPrefs.GetInt("video")];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("nextScene"));
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("nextScene"));
    }
}
