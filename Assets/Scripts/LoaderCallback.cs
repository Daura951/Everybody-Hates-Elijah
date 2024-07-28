using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    private AsyncOperation async;
    public static int targetScene;
    private Image image;

    private void Awake()
    {
        image = transform.GetComponent<Image>();
    }

    private void Update()
    {
        image.fillAmount = GetLoadingProgress();
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(5);
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return null;
        async = SceneManager.LoadSceneAsync(targetScene);
        while (!async.isDone)
            yield return null;
    }

    public float GetLoadingProgress()
    {
        if (async != null)
        {
            return async.progress;
        }
        else
            return 0f;
    }
}
