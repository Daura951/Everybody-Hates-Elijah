public class GameObjectPosition
{
    public enum LayerPosition  { BACKGROUND, MIDDLEGROUND, FOREGROUND }
    public float x { get; set; }
    public float y { get; set; }
    public string gameObjectName { get; set; }
    public LayerPosition layerPosition { get; set; } = LayerPosition.MIDDLEGROUND;
}
