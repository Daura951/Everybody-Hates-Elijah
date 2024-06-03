using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeButton : MonoBehaviour
{

    public LevelEditorMouse.LevelManipulation edit_mode;

    public delegate void OnLevelManipulationChange(LevelEditorMouse.LevelManipulation newValue);
    public static OnLevelManipulationChange onLevelManipulationChange;

    public void changeMode()
    {
        onLevelManipulationChange?.Invoke(edit_mode);
    }
}
