using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelEditorHotbarButtonController : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public void set_sprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void set_text(string text)
    {
        textMeshProUGUI.text = text;
    }
}
