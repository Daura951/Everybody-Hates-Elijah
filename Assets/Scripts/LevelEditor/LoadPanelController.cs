using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelController : MonoBehaviour
{
    private string loadPath = "";
    private DirectoryInfo d;

    public delegate void OnLevelLoad();
    public static OnLevelLoad onLevelLoad;
    public delegate void OnLevelLoaded(string loaded_level_name);
    public static OnLevelLoaded onLevelLoaded;

    public TileMapGenerator tileMapGenerator;
    public GameObject ContentObject;

    private void OnEnable()
    {
        LevelEditorManager.onLevelSaved += (string l) => refreshButtons();
    }
    
    void Start()
    {
        loadPath = Application.dataPath + "/LevelData/";
        d = new DirectoryInfo(loadPath);
        loadLevelButtons();
    }

    private void refreshButtons()
    {
        foreach (Transform child in ContentObject.transform)
        {
            Destroy(child.gameObject);
        }

        loadLevelButtons();
    }

    private void loadLevelButtons()
    {
        foreach(var file in d.GetFiles("*.json")) 
        {
            TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
            GameObject button = TMP_DefaultControls.CreateButton(resources);
            button.transform.parent = ContentObject.transform;
            string file_name = file.Name.Replace(".json", "");
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(file_name);
            button.GetComponent<Button>().onClick.AddListener(() => onLevelButtonClick(file_name));
        }
    }

    private void onLevelButtonClick(string levelName)
    {
        Debug.Log("loading --> " + levelName);
        onLevelLoad?.Invoke();
        tileMapGenerator.setAndGenerate(levelName, TileMapGenerator.generateType.sprite);
        onLevelLoaded?.Invoke(levelName);
    }

    private void OnDisable()
    {
        LevelEditorManager.onLevelSaved -= (string l) => refreshButtons();
    }
}
