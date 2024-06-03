using TMPro;
using UnityEngine;

public class ModePanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    
    private void OnEnable()
    {
        // listen for event thrown by button
        EditModeButton.onLevelManipulationChange += (LevelEditorMouse.LevelManipulation lm) => { ChangeModeText(lm.ToString()); };
    }

    private void ChangeModeText(string text)
    {
        textMeshProUGUI.text = text;
    }

    private void OnDisable()
    {
        // detach event
        EditModeButton.onLevelManipulationChange -= (LevelEditorMouse.LevelManipulation lm) => { ChangeModeText(lm.ToString()); };
    }
}
