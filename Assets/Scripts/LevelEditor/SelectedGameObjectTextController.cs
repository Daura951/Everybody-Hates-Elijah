using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectedGameObjectTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmpro;
    private void OnEnable()
    {
        LevelEditorMouse.onSelectedGameObjectChanged += (string text) => { tmpro.text = text; };
    }

    private void OnDisable()
    {
        LevelEditorMouse.onSelectedGameObjectChanged -= (string text) => { tmpro.text = text; };

    }
}
