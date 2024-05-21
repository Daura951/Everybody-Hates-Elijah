using TMPro;
using UnityEngine;

public class ZoomLevelTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmpro;
    private void OnEnable()
    {
        LevelEditorCameraMovement.onZoomLevelChanged += (float level) => 
        { tmpro.text = "Zoom: " + level.ToString(); };
    }

    private void OnDisable()
    {
        LevelEditorCameraMovement.onZoomLevelChanged -= (float level) => 
        { tmpro.text = "Zoom: " + level.ToString(); };

    }
}
