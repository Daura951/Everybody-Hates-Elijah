using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    private float time;
    private bool stopTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopTimer)
            time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stopTimer = true;
        print(time);
        PlayerPrefs.SetFloat("endTime", time);
        SceneManager.LoadScene(3);
    }
}
