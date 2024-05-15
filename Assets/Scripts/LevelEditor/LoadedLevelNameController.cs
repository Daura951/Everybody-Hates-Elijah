using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadedLevelNameController : MonoBehaviour
{
    private void OnEnable()
    {
        LoadPanelController.onLevelLoaded += (string loaded_level_name) => updateName(loaded_level_name);
        LevelEditorManager.onLevelNew += () => updateName("");
        LevelEditorManager.onLevelSaved += (string level_name) => updateName(level_name);
    }
    private void updateName(string loaded_level_name)
    {
        gameObject.transform.GetComponent<TextMeshProUGUI>().text = loaded_level_name;
    }

    private void OnDisable()
    {
        LoadPanelController.onLevelLoaded -= (string loaded_level_name) => updateName(loaded_level_name);
        LevelEditorManager.onLevelNew -= () => updateName("");
        LevelEditorManager.onLevelSaved -= (string level_name) => updateName(level_name);
    }
}
