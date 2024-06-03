using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LevelEditorManager : MonoBehaviour
{
    public enum EditorMode { PLAYMODE, EDITMODE };

    private EditorMode m_EditorMode = EditorMode.EDITMODE;
    [HideInInspector]
    public bool playerPlaced = false;
    [HideInInspector]
    public bool saveLoadMenuOpen = false;
    public Animator optionUIAnimation;
    public Animator saveUIAnimation;
    public Animator loadUIAnimation;
    public Camera levelEditorCamera;
    public SpriteAndGameObject mouseObject;
    public LevelEditorMouse user;
    public Sprite playerMarker;
    public TMP_InputField levelNameSave;
    public TMP_Text levelMessage;
    public Animator messageAnim;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    public TileMapLevelFileWriter tileMapLevelFileWriter;
    public TileMapGenerator tileMapGenerator;

    public delegate void OnLevelSaved(string level_name);
    public static OnLevelSaved onLevelSaved;
    private string loaded_level;

    public delegate void OnEnterPlayMode();
    public static OnEnterPlayMode onEnterPlayMode;
    public delegate void OnEnterEditMode();
    public static OnEnterEditMode onEnterEditMode;

    public delegate void OnLevelNew();
    public static OnLevelNew onLevelNew;

    private void OnEnable()
    {
        LoadPanelController.onLevelLoad += BeforeLevelLoad;
        LoadPanelController.onLevelLoaded += (string loaded_level_name) => AfterLevelLoaded(loaded_level_name);
    }

    public EditorMode getEditorMode()
    {
        return m_EditorMode;
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
        user.setSelectedGameObject(user.tileGameObjects.getNext(user.selectedGameObjectName));
        mouseObject = user.selectedGameObject;
    }
    public void SelectPreviousGameObject()
    {
        user.setSelectedGameObject(user.tileGameObjects.getPrevious(user.selectedGameObjectName));
        mouseObject = user.selectedGameObject;
    }
    public void ChoosePlayerStart()
    {
        SpriteAndGameObject playerMarker = user.selectedGameObject = user.tileGameObjects.get("Player");
        user.setSelectedGameObject(playerMarker);
        mouseObject = playerMarker;
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

        Dictionary<string, List<(GameObject, GameObjectPosition.LayerPosition)>> gameObjectLayers = new Dictionary<string, List<(GameObject, GameObjectPosition.LayerPosition)>>();
        List<string> layerNames = new List<string>();

        foreach (Transform child in tileMapGenerator.stagingArea.transform)
        {

            string name = child.name;
            string child_layer = tileMapGenerator.get_gameobject_layer_name(name);

            if (!gameObjectLayers.ContainsKey(child_layer))
            {
                layerNames.Add(child_layer);
                Debug.Log(child_layer);
                gameObjectLayers.Add(child_layer, new List<(GameObject, GameObjectPosition.LayerPosition)>());
                
            }
            gameObjectLayers[child_layer].Add((child.gameObject, GameObjectPosition.LayerPosition.MIDDLEGROUND));
        }

        foreach (Transform child in tileMapGenerator.stagingAreaBackground.transform)
        {
            string name = child.name;
            string child_layer = tileMapGenerator.get_gameobject_layer_name(name);
            if (!gameObjectLayers.ContainsKey(child_layer))
            {
                layerNames.Add(child_layer);
                Debug.Log(child_layer);
                gameObjectLayers.Add(child_layer, new List<(GameObject, GameObjectPosition.LayerPosition)>());
            }
            gameObjectLayers[child_layer].Add((child.gameObject, GameObjectPosition.LayerPosition.BACKGROUND));
        }

        foreach (Transform child in tileMapGenerator.stagingAreaForeground.transform)
        {
            string name = child.name;
            string child_layer = tileMapGenerator.get_gameobject_layer_name(name);
            if (!gameObjectLayers.ContainsKey(child_layer))
            {
                layerNames.Add(child_layer);
                Debug.Log(child_layer);
                gameObjectLayers.Add(child_layer, new List<(GameObject, GameObjectPosition.LayerPosition)>());
            }
            gameObjectLayers[child_layer].Add((child.gameObject, GameObjectPosition.LayerPosition.FOREGROUND));
        }

        tileMapLevelFileWriter.Save_File(level_name_text, gameObjectLayers, layerNames);

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
        user.clearStagingAreas();
    }

    private void LoadUpLevel()
    {
        user.clearStagingAreas();
        tileMapGenerator.setAndGenerate(
            loaded_level,
            m_EditorMode == EditorMode.EDITMODE ? 
            TileMapGenerator.generateType.gameObject : 
            TileMapGenerator.generateType.sprite
        );

        if (m_EditorMode == EditorMode.EDITMODE)
        {
            m_EditorMode = EditorMode.PLAYMODE;
            onEnterPlayMode?.Invoke();
            return;
        }
        if (m_EditorMode == EditorMode.PLAYMODE)
        {
            m_EditorMode = EditorMode.EDITMODE;
            onEnterEditMode?.Invoke();
        }

    }

    public void ResetPlayLevel()
    {
        if (m_EditorMode == EditorMode.PLAYMODE)
        {
            user.clearStagingAreas();
            tileMapGenerator.setAndGenerate(
                loaded_level,
                TileMapGenerator.generateType.gameObject
                );
        }
    }

    public void PlayLevel()
    {
        if (m_EditorMode == EditorMode.PLAYMODE)
        {
            return;
        }
        if (loaded_level == null || loaded_level == "")
        {
            ChooseSave();
            return;
        }
        levelEditorCamera.transform.gameObject.SetActive(false);
        SaveLevel(loaded_level);
        LoadUpLevel();
    }

    public void StopPlayLevel()
    {
        if (m_EditorMode == EditorMode.EDITMODE)
        {
            return;
        }
        if (loaded_level == null || loaded_level == "")
        {
            return;
        }
        levelEditorCamera.transform.gameObject.SetActive(true);
        LoadUpLevel();
    }

    public void newLevel()
    {
        m_EditorMode = EditorMode.EDITMODE;
        user.clearStagingAreas();
        loaded_level = "";
        onLevelNew?.Invoke();
    }


    private void OnDisable()
    {
        LoadPanelController.onLevelLoaded -= AfterLevelLoaded;
        LoadPanelController.onLevelLoad -= BeforeLevelLoad;
    }

}
