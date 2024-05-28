using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectSelectorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject button_parent;

    [SerializeField]
    private GameObject button;

    [SerializeField]
    private TileGameObjects tileGameObjectDict;

    [SerializeField]
    private LevelEditorMouse mouse;

    void Start()
    {
        set_up_hot_bar_buttons();   
    }

    private void onHotBarButtonClick(SpriteAndGameObject spriteAndGameObject)
    {
        mouse.setSelectedGameObject(spriteAndGameObject);
    }

    private void set_up_hot_bar_buttons()
    {
        foreach (SpriteAndGameObject spriteAndGameObject in tileGameObjectDict.getAll())
        {
            GameObject instanced_button = Instantiate(button, button_parent.transform);
            string object_name = spriteAndGameObject.obj.name;
            LevelEditorHotbarButtonController button_controller = instanced_button.GetComponent<LevelEditorHotbarButtonController>();
            button_controller.set_text(object_name);
            button_controller.set_sprite(spriteAndGameObject.sprite);
            instanced_button.GetComponent<Button>().onClick.AddListener(() => onHotBarButtonClick(spriteAndGameObject));
        }
    }
}
