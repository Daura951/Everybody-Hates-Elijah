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
    public SpriteAndGameObject mouseObject;
    public LevelEditorMouse user;
    public Sprite playerMarker;
    public TMP_InputField levelNameSave;
    public TMP_Text levelMessage;
    public TMP_Text GameObjectNameDisplay;
    public Animator messageAnim;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    public TileMapLevelFileWriter tileMapLevelFileWriter;
    public TileMapGenerator tileMapGenerator;

    public delegate void OnLevelSaved(string level_name);
    public static OnLevelSaved onLevelSaved;
    private string loaded_level;

    public delegate void OnLevelNew();
    public static OnLevelNew onLevelNew;

    private void OnEnable()
    {
        LoadPanelController.onLevelLoad += BeforeLevelLoad;
        LoadPanelController.onLevelLoaded += (string loaded_level_name) => AfterLevelLoaded(loaded_level_name);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObjectNameDisplay.text = user.selectedGameObject.obj.name;
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
        user.selectedGameObject = user.tileGameObjects. getNext(user.selectedGameObjectName);
        user.selectedGameObjectName = user.selectedGameObject.obj.name;
        mouseObject = user.selectedGameObject;
        GameObjectNameDisplay.text = user.selectedGameObject.obj.name;
    }
    public void SelectPreviousGameObject()
    {
        user.selectedGameObject = user.tileGameObjects.getPrevious(user.selectedGameObjectName);
        user.selectedGameObjectName = user.selectedGameObject.obj.name;
        mouseObject = user.selectedGameObject;
        GameObjectNameDisplay.text = user.selectedGameObject.obj.name;
    }
    public void ChoosePlayerStart()
    {
        SpriteAndGameObject playerMarker = user.selectedGameObject = user.tileGameObjects.get("Player");
        user.selectedGameObject = playerMarker;
        user.selectedGameObjectName = playerMarker.obj.name;
        mouseObject = playerMarker;
    }

    public void ChooseCreate()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Create;
        user.spriteRenderer.enabled = true;
    }
    public void ChooseRotate()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Rotate;
        user.spriteRenderer.enabled = false;
    }
    public void ChooseDestroy()
    {
        user.manipulateOption = LevelEditorMouse.LevelManipulation.Destroy;
        user.spriteRenderer.enabled = false;
    }

    public void SaveLevelAs()
    {
        SaveLevel(levelNameSave.text, true);
    }

    public void onSaveButtonClicked()
    {
        SaveLevel(loaded_level);
    }

    public void SaveLevel(string level_name_text, bool is_save_as = false)
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

        if (is_save_as)
        {
            saveUIAnimation.SetTrigger("SaveLoadIn");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false;
            levelNameSave.text = "";
            levelNameSave.DeactivateInputField();
        }
        loaded_level = level_name_text;
        levelMessage.text = levelNameSave.text + " saved to LevelData folder.";
        messageAnim.SetTrigger("SaveLoadOut");
        onLevelSaved.Invoke(level_name_text);
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

    public void PlayLevel()
    {
        if (loaded_level == null || loaded_level == "")
        {
            ChooseSave();
            return;
        }
        SaveLevel(loaded_level);
        user.clearStagingArea();
        tileMapGenerator.setAndGenerate(loaded_level, TileMapGenerator.generateType.gameObject);
    }

    public void StopPlayLevel()
    {
        if (loaded_level == null || loaded_level == "")
        {
            return;
        }
        user.clearStagingArea();
        tileMapGenerator.setAndGenerate(loaded_level, TileMapGenerator.generateType.sprite);
    }

    public void newLevel()
    {
        user.clearStagingArea();
        loaded_level = "";
        onLevelNew?.Invoke();
    }


    private void OnDisable()
    {
        LoadPanelController.onLevelLoaded -= AfterLevelLoaded;
        LoadPanelController.onLevelLoad -= BeforeLevelLoad;
    }

}
