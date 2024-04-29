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
    public Slider rotSlider;
    public GameObject rotUI;
    public TMP_InputField levelNameSave;
    public TMP_InputField levelNameLoad;
    public TMP_Text levelMessage;
    public TMP_Text GameObjectNameDisplay;
    public Animator messageAnim;
    private bool itemPositionIn = true;
    private bool optionPositionIn = true;
    private bool saveLoadPositionIn = false;
    public  TileMapLevelFileWriter tileMapLevelFileWriter;

    // Start is called before the first frame update
    void Start()
    {
        GameObjectNameDisplay.text = user.selectedGameObject.name;
        rotSlider.onValueChanged.AddListener(delegate {
            RotationValueChange();
        });
    }

    void RotationValueChange()
    {
        //TODO: Remove Rotating
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

    public void SaveLevel()
    {
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

        tileMapLevelFileWriter.save_file(levelNameSave.text, gameObjectLayers, layerNames);

        saveUIAnimation.SetTrigger("SaveLoadOut");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;
        levelNameSave.text = "";
        levelNameSave.DeactivateInputField();
        levelMessage.text = levelNameSave.text + " saved to LevelData folder.";
        messageAnim.Play("MessageFade", 0, 0);
    }

    public void LoadLevel()
    {
        string folder = Application.dataPath + "/LevelData/";
        string levelFile = "";
        if (levelNameLoad.text == "")
            levelFile = "new_level.json";
        else
            levelFile = levelNameLoad.text + ".json";

        string path = Path.Combine(folder, levelFile);

        if (File.Exists(path))
        {
            // TODO: Write Loader and load in level from here
        }
        else
        {
            loadUIAnimation.SetTrigger("SaveLoadOut");
            saveLoadPositionIn = false;
            saveLoadMenuOpen = false;
            levelMessage.text = levelFile + " could not be found!";
            messageAnim.Play("MessageFade", 0, 0);
            levelNameLoad.DeactivateInputField();
        }
    }

    void CreateFromFile()
    {
        // TODO: Fix this here


        levelNameLoad.text = "";
        levelNameLoad.DeactivateInputField();

        loadUIAnimation.SetTrigger("SaveLoadOut");
        saveLoadPositionIn = false;
        saveLoadMenuOpen = false;

        levelMessage.text = "Level loading...done.";
        messageAnim.Play("MessageFade", 0, 0);
    }


}
