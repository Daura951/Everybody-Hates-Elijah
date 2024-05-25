using TMPro;
using UnityEngine;

public class LayerSwitchButtonController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public delegate void OnLayerSwitchUpdate(GameObjectPosition.LayerPosition layerPosition);
    public static event OnLayerSwitchUpdate onLayerSwitchUpdate;

    private GameObjectPosition.LayerPosition current_layer = GameObjectPosition.LayerPosition.MIDDLEGROUND;
    private string layer_text = "Middleground";
    private void Start()
    {
        updateText(layer_text);
        onLayerSwitchUpdate?.Invoke(current_layer);
    }
    public void  onUpdateLayer ()
    {
        string layer_text = "";
        switch(current_layer)
        {
            case GameObjectPosition.LayerPosition.MIDDLEGROUND:
                current_layer = GameObjectPosition.LayerPosition.BACKGROUND;
                layer_text = "Background";
                break;
            case GameObjectPosition.LayerPosition.BACKGROUND:
                current_layer = GameObjectPosition.LayerPosition.FOREGROUND;
                layer_text = "Foreground";
                break;
            case GameObjectPosition.LayerPosition.FOREGROUND:
                current_layer = GameObjectPosition.LayerPosition.MIDDLEGROUND;
                layer_text = "Middleground";
                break;
        }
        updateText(layer_text);
        onLayerSwitchUpdate?.Invoke(current_layer);
    }

    private void updateText(string text)
    {
        textMeshProUGUI.SetText("Layer Level: " + text);
    }
}
