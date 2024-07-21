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
    public float holdTime;
    private float currentHoldTime = 0.0f;

    public SpriteRenderer skipSprite;
    void Start()
    {
        player.loopPointReached += OnVideoEnd;
        skipSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        player.clip = videos[PlayerPrefs.GetInt("video")];
        PlayerPrefs.SetInt("loadGameIndex", SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey|| Input.anyKey)
        {
            currentHoldTime += Time.deltaTime;
            skipSprite.color = new Color(1.0f, 1.0f, 1.0f, skipSprite.color.a + Time.deltaTime);
        }
        else
        {
            if (currentHoldTime >= 0.0f)
            {
                currentHoldTime -= Time.deltaTime;
                if(currentHoldTime >= holdTime*.5f)
                skipSprite.color = new Color(1.0f, 1.0f, 1.0f, skipSprite.color.a - Time.deltaTime);
            }
        }

        if(currentHoldTime >= holdTime)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("nextScene"));
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("nextScene"));
    }
}
