using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TileMapLevelFileWriter : MonoBehaviour
{
    private string fullPath = "";
 
    private void Awake()
    {
        fullPath = Application.dataPath + "/LevelData/";
    }
    
    public void save_file(string filename, Dictionary<string, List<GameObject>> gameObjectLayers, List<string> layerNames)
    {
        List<string> lines =  processGameObjects(gameObjectLayers, layerNames );
        writeFile(lines, filename);
    }
    private List<string> processGameObjects(
        Dictionary<string, List<GameObject>> gameObjectLayers,
        List<string> layerNames
      )
    {
        List<string> lines = new List<string>();
        string starting_lines ="{\r\n  \"tileSize\": 32,\r\n  \"mapWidth\": 4,\r\n  \"mapHeight\": 4,\r\n  \"layers\": [";
        lines.Add(starting_lines);

        string lastLayer = layerNames.Last();
        foreach (var layer in layerNames)
        {
            List<GameObject> gameObjects = gameObjectLayers[layer];
            GameObject LastGameObject = gameObjects.Last();
            string layer_object_start = $"{{ \"name\": \"{layer}\",\r\n \"gameObjectPositions\": [";
            lines.Add(layer_object_start);
            foreach (GameObject gameObject in gameObjects)
            {
                string newLine = $"{{ " +
                    $"\"gameObjectName\": \"{gameObject.name.Replace("(Clone)", "")}\", " +
                    $"\"x\":{gameObject.transform.position.x}, " +
                    $"\"y\":{gameObject.transform.position.y} }}";
                
                if (!gameObject.Equals(LastGameObject))
                {
                    newLine += ",";
                }
                lines.Add(newLine);
            }
            string layer_object_end = "]\r\n}";
            if (!layer.Equals(lastLayer))
            {
                layer_object_end += ",";
            }
            lines.Add(layer_object_end);
        }
        string last_lines = "]\r\n}";
        lines.Add(last_lines);

        return lines;
    }

    private void writeFile(List<string> lines, string filename)
    {
        if (filename == "")
            filename = "new_level.json";
        else
            filename += ".json";
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        var filePath = Path.Combine(fullPath, filename);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var fs = new FileStream(filePath, FileMode.CreateNew);
        var stream = new StreamWriter(fs);
        foreach (var line in lines)
        {
            stream.WriteLine(line);
        }
        stream.Close();
    }
}
