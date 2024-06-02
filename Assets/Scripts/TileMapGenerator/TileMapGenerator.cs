using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;
public class TileMapGenerator : MonoBehaviour
{
    public enum generateType { sprite, gameObject }

    public UnityEngine.TextAsset mapFile;
    // Start is called before the first frame update

    [SerializeField]
    private TileGameObjects tileGameObjectDict;

    [SerializeField]
    public GameObject stagingArea;

    [SerializeField]
    public GameObject stagingAreaBackground;

    [SerializeField]
    public GameObject stagingAreaForeground;

    void Start()
    {
        if (mapFile != null)
        {
            string mapFileJson = mapFile.text.ToString();
            generate(mapFileJson, generateType.sprite);
        }
    }

    private void generate(string mapFileJson, generateType generateType)
    {
        int objectCount = 0;
        if (mapFileJson == null || mapFileJson == "")
        {
            return;
        }
        LevelMap levelMap = JsonConvert.DeserializeObject<LevelMap>(mapFileJson);

        int tileSize = levelMap.tileSize;
        int mapWidth = levelMap.mapWidth;
        int mapHeight = levelMap.mapHeight;

        foreach (LevelLayer layer in levelMap.layers)
        {
            UnityEngine.Debug.Log("Layer Name: " + layer.name);
            foreach (GameObjectPosition gameObjectPosition in layer.gameObjectPositions)
            {

                GameObject obj = tileGameObjectDict.getGameObject(gameObjectPosition.gameObjectName);
                Vector3 pos = new Vector3(
                    gameObjectPosition.x,
                    gameObjectPosition.y,
                    obj.transform.position.z
                );

                Transform parent_transform = get_parent_object(gameObjectPosition).transform;
                if (generateType == generateType.gameObject)
                {
                    Instantiate(obj, pos, Quaternion.identity, parent_transform);
                }
                else if (generateType == generateType.sprite)
                {
                    objectCount += 1;
                    GameObject newObj = new GameObject(gameObjectPosition.gameObjectName);
                    SpriteRenderer sr = newObj.AddComponent<SpriteRenderer>();
                    sr.sprite = tileGameObjectDict.getSprite(gameObjectPosition.gameObjectName);
 
                    LevelEditorMoveableObject moveable = newObj.AddComponent<LevelEditorMoveableObject>();
                    moveable.myRenderer = sr;

                    Canvas canvas = newObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.WorldSpace;
                    canvas.worldCamera = Camera.main;

                    GameObject childObj = new GameObject(gameObjectPosition.gameObjectName + "Text");
                    childObj.transform.parent = newObj.transform;
                    childObj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                    ContentSizeFitter csf = childObj.AddComponent<ContentSizeFitter>();
                    csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


                    TextMeshProUGUI tm = childObj.AddComponent<TextMeshProUGUI>();
                    tm.outlineColor = Color.black;
                    tm.outlineWidth = 0.14f;
                    tm.SetText(objectCount.ToString());
                    tm.fontSize = 5;

                    newObj.transform.position = pos;
                    newObj.transform.parent = parent_transform;
                }
            }
        }

    }

    private GameObject get_parent_object(GameObjectPosition position)
    {
        switch (position.layerPosition) 
        {
            case GameObjectPosition.LayerPosition.MIDDLEGROUND:
                return stagingArea;
            case GameObjectPosition.LayerPosition.BACKGROUND:
                return stagingAreaBackground;
            case GameObjectPosition.LayerPosition.FOREGROUND:
                return stagingAreaForeground;
            default:
                return stagingArea;
        }
    }

    public void setAndGenerate(string level_name, generateType generateType)
    {
        string full_level_name = level_name + ".json";
        string mapFileJson = "";
        string loadPath = Application.dataPath + "/LevelData/";
        try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(loadPath + full_level_name))
            {
                // Read the stream as a string into the mapFileJson variable.
                mapFileJson = sr.ReadToEnd();
            }
        }
        catch (IOException e)
        {
            Debug.LogError("The file could not be read: " + e.Message);
        }

        generate(mapFileJson, generateType);

    }

    public string get_gameobject_layer_name(string object_name)
    {
        return tileGameObjectDict.get_layer_name(object_name);
    }

}
