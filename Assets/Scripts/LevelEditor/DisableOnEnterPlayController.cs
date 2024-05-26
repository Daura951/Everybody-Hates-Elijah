
using UnityEngine;

public class EnableOnEnterPlayController : MonoBehaviour
{
    private void OnEnable()
    {
        LevelEditorManager.onEnterPlayMode += onEnterPlayMode;
        LevelEditorManager.onEnterEditMode += onEnterEditMode;
    }
    private void onEnterPlayMode()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void onEnterEditMode()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        LevelEditorManager.onEnterEditMode -= onEnterEditMode;
        LevelEditorManager.onEnterPlayMode -= onEnterPlayMode;
    }

}
