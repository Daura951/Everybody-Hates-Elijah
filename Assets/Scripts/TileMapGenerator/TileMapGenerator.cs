using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
public class TileMapGenerator : MonoBehaviour
{
    public enum generateType { sprite, gameObject }

    public UnityEngine.TextAsset mapFile;
    // Start is called before the first frame update

    [SerializeField]
    private TileGameObjects tileGameObjectDict;

    [SerializeField]
    private GameObject stagingArea;

    void Start()
    {
        if(mapFile != null)
        {
            string mapFileJson = mapFile.text.ToString();
            generate(mapFileJson, generateType.sprite);
        }
    }

    private void generate(string mapFileJson, generateType generateType )
    {
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
                Vector3 pos = new Vector3(
                            gameObjectPosition.x,
                            gameObjectPosition.y,
                            0f
                        );
                if (generateType == generateType.gameObject)
                {
                    GameObject obj = tileGameObjectDict.getGameObject(gameObjectPosition.gameObjectName); 
                    Instantiate(obj, pos, Quaternion.identity, stagingArea.transform);
                }
                else if (generateType == generateType.sprite)
                {
                    GameObject newObj = new GameObject(gameObjectPosition.gameObjectName);
                    SpriteRenderer sr = newObj.AddComponent<SpriteRenderer>();
                    sr.sprite = tileGameObjectDict.getSprite(gameObjectPosition.gameObjectName);
                    LevelEditorMoveableObject moveable = newObj.AddComponent<LevelEditorMoveableObject>();
                    moveable.myRenderer = sr;

                    newObj.transform.position = pos;
                    newObj.transform.parent = stagingArea.transform;
                }
            }
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

}
