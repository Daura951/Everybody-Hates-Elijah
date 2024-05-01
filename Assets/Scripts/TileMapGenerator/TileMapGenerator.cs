using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
public class TileMapGenerator : MonoBehaviour
{
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
            generate(mapFileJson);
        }
    }

    private void generate(string mapFileJson)
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
                GameObject obj = tileGameObjectDict.getGameObject(gameObjectPosition.gameObjectName); 
                Instantiate(obj, new Vector3(gameObjectPosition.x, gameObjectPosition.y, 0f), Quaternion.identity, stagingArea.transform);
            }
        }

    }

    public void setAndGenerate(string level_name)
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

        generate(mapFileJson);

    }

}
