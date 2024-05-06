using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LevelEditorManager : MonoBehaviour
{
    [HideInInspector]
    public bool playerPlaced = false;
    [HideInInspector]
    public bool saveLoadMenuOpen = false;
    public Animator optionUIAnimation;
    public Animator saveUIAnimation;
    public Animator loadUIAnimation;
    public GameObject mouseObject;
    public LevelEditorMouse user;
    public Sprite playerMarker;
    public GameObject rotUI;
    public TMP_InputField levelNameSave;
    public TMP_Text levelMessage;
    public TMP_Text GameObjectNameDisplay;
    public Animator messageAnim;
    private bool itemPositionIn = true;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    public TileMapLevelFileWriter tileMapLevelFileWriter;

    public delegate void OnLevelSaved();
    public static OnLevelSaved onLevelSaved;
    private string loaded_level;

    private void OnEnable()
    {
        LoadPanelController.onLevelLoad += BeforeLevelLoad;
        LoadPanelController.onLevelLoaded += (string loaded_level_name) => AfterLevelLoaded(loaded_level_name);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObjectNameDisplay.text = user.selectedGameObject.name;
    }

    public void SlideOptionMenu()
    {
        if (optionPositionIn == false)
        {
            optionUIAnimation.SetTrigger("OptionMenuIn");
            optionPositionIn = true;
        }
        else
        {
            optionUIAnimation.SetTrigger("OptionMenuOut");
            optionPositionIn = false;
        }
    }

    public void ChooseSave()
    {
        if (saveLoadPositionIn == false)
        {
            saveUIAnimation.SetTrigger("SaveLoadOut");
            saveLoadPositionIn = true;
            saveLoadMenuOpen = true;
        }
        else
        {
            saveUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false;
        }
    }
    public void ChooseLoad()
    {
        if (saveLoadPositionIn == false)
        {
            loadUIAnimation.SetTrigger("SaveLoadOut");
            saveLoadPositionIn = true;
            saveLoadMenuOpen = true;
        }
        else
        {
            loadUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false;
        }
    }

    public void SelectNextGameObject()
    {
        // controls for selecting game object
        user.selectedGameObject = user.tileGameObjects. getNextGameObject(user.selectedGameObjectName);
        user.selectedGameObjectName = user.selectedGameObject.name;
        mouseObject = user.selectedGameObject;
        GameObjectNameDisplay.text = user.selectedGameObject.name;
    }
    public void SelectPreviousGameObject()
    {
        // controls for selecting game object
        user.selectedGameObject = user.tileGameObjects.getPreviousGameObject(user.selectedGameObjectName);
        user.selectedGameObjectName = user.selectedGameObject.name;
        mouseObject = user.selectedGameObject;
        GameObjectNameDisplay.text = user.selectedGameObject.name;
    }
    public void ChoosePlayerStart()
    {
        GameObject playerMarker = user.selectedGameObject = user.tileGameObjects.getGameObject("Player");
        user.selectedGameObject = playerMarker;
        user.selectedGameObjectName = playerMarker.name;
        mouseObject = playerMarker;
    }

    public void ChooseCreate()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Create;
        user.spriteRenderer.enabled = true;
        rotUI.SetActive(false);
    }
    public void ChooseRotate()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Rotate;
        user.spriteRenderer.enabled = false;
        rotUI.SetActive(true);
    }
    public void ChooseDestroy()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Destroy;
        user.spriteRenderer.enabled = false;
        rotUI.SetActive(false);
    }

    public void SaveLevelAs()
    {
        SaveLevel(levelNameSave.text);
    }

    public void SaveLevel(string level_name_text)
    {
        if (level_name_text == null || level_name_text == "")
        {
            if (loaded_level == null || loaded_level == "")
            {
                ChooseSave();
                return;
            }
            level_name_text = loaded_level;
        }

        Dictionary<string, List<GameObject>> gameObjectLayers = new Dictionary<string, List<GameObject>>();
        List<string> layerNames = new List<string>();

        foreach (Transform child in user.stagingArea.transform)
        {
            string child_layer = LayerMask.LayerToName(child.gameObject.layer);
            if (!gameObjectLayers.ContainsKey(child_layer))
            {
                layerNames.Add(child_layer);
                gameObjectLayers.Add(child_layer, new List<GameObject>());

            }
            gameObjectLayers[child_layer].Add(child.gameObject);
        }

        tileMapLevelFileWriter.save_file(level_name_text, gameObjectLayers, layerNames);

        saveUIAnimation.SetTrigger("SaveLoadIn");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;
        levelNameSave.text = "";
        levelNameSave.DeactivateInputField();
        levelMessage.text = levelNameSave.text + " saved to LevelData folder.";
        messageAnim.SetTrigger("SaveLoadOut");
        onLevelSaved.Invoke();
    } 

    public void AfterLevelLoaded(string loaded_level_name)
    {
        loaded_level = loaded_level_name;
        loadUIAnimation.SetTrigger("SaveLoadIn");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;
        levelMessage.text = "Level Loaded: " + loaded_level_name;
        messageAnim.SetTrigger("SaveLoadOut");
    }

    public void BeforeLevelLoad()
    {
        user.clearStagingArea();
    }

    private void OnDisable()
    {
        LoadPanelController.onLevelLoaded -= AfterLevelLoaded;
        LoadPanelController.onLevelLoad -= BeforeLevelLoad;
    }

}
