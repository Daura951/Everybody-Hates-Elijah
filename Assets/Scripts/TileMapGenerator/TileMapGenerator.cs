using UnityEngine;
using Newtonsoft.Json;
public class TileMapGenerator : MonoBehaviour
{
    public UnityEngine.TextAsset mapFile;
    // Start is called before the first frame update

    [SerializeField]
    private TileGameObjects tileGameObjectDict;

    void Start()
    {
        
        generate();
    }

    private void generate()
    {
        string mapFileJson = mapFile?.text.ToString();
        LevelMap levelMap = JsonConvert.DeserializeObject<LevelMap>(mapFileJson);

        // Have a huge prefab map and attach it here.
        // It is a little dangerous, but we can throw a useful error
        // if we are missing a prefab object

        int tileSize = levelMap.tileSize;
        int mapWidth = levelMap.mapWidth;
        int mapHeight = levelMap.mapHeight;

        foreach (LevelLayer layer in levelMap.layers)
        {
            UnityEngine.Debug.Log("Layer Name: " + layer.name);
            foreach (GameObjectPosition gameObjectPosition in layer.gameObjectPositions)
            {
                UnityEngine.Debug.Log("x: " + gameObjectPosition.x);
                UnityEngine.Debug.Log("y: " + gameObjectPosition.y);
                UnityEngine.Debug.Log("Game Object: " + gameObjectPosition.gameObjectName);
                GameObject obj = tileGameObjectDict.getGameObject(gameObjectPosition.gameObjectName); 
                Instantiate(obj, new Vector3(gameObjectPosition.x, gameObjectPosition.y, 0f), Quaternion.identity);
            }
        }

    }

}
