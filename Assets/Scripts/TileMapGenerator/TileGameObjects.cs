using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGameObjects : MonoBehaviour
{
    [SerializeField]
    string thisObjectName;

    [SerializeField]
    public TileGameObjectDictionary newDict;

    private Dictionary<string, SpriteAndGameObject> tileGameObjectDictionary;

    private void Awake()
    {
        tileGameObjectDictionary = newDict.ToDictionary();
    }

    public GameObject getGameObject(string key)
    {
        return get(key).obj;
    }

    public Sprite getSprite(string key)
    {
        return get(key).sprite;
    }

    public SpriteAndGameObject get(string key)
    {
        if (!tileGameObjectDictionary.ContainsKey(key))
        {
            Debug.LogError("Missing '" + key + "' tile in the tileGameObjects dictionary.");
        }
        return tileGameObjectDictionary[key];
    }

    public string get_layer_name(string key)
    {
        return LayerMask.LayerToName(get(key).obj.layer);
    }

    public GameObject getNextGameObject(string key)
    {
        return getNext(key)?.obj;
    }

    public GameObject getPreviousGameObject(string key)
    {
        return getPrevious(key)?.obj;
    }
    public Sprite getNextSprite(string key)
    {
        return getNext(key)?.sprite;
    }

    public Sprite getPreviousSprite(string key)
    {
        return getPrevious(key)?.sprite;
    }

    public SpriteAndGameObject getFirst()
    {
        return tileGameObjectDictionary.First().Value;
    }

    public List<SpriteAndGameObject> getAll()
    {
        return tileGameObjectDictionary.Values.ToList();
    }

    public SpriteAndGameObject getNext(string key)
    {
        string firstKey = "";
        string getKey = "";
        bool returnNext = false;
        foreach (KeyValuePair<string, SpriteAndGameObject> entry in tileGameObjectDictionary)
        {
            if (firstKey == "")
            {
                firstKey = entry.Key;
                getKey = entry.Key;
            }
            // do something with entry.Value or entry.Key
            if (returnNext == true)
            {
                getKey = entry.Key;
                break;
            }
            if (entry.Key == key)
            {
                returnNext = true;
            }
        }
        return get(getKey);
    }
    public SpriteAndGameObject getPrevious(string key)
    {
        bool getLast = false;
        string firstKey = "";
        string getKey = "";
        bool returnNext = false;
        foreach (KeyValuePair<string, SpriteAndGameObject> entry in tileGameObjectDictionary)
        {
            if (firstKey == "")
            {
                firstKey = entry.Key;
            }
            if (firstKey == key)
            {
                getLast = true;
            }
            // do something with entry.Value or entry.Key
            if (getLast == false)
            {
                if (entry.Key == key)
                {
                    returnNext = true;
                }
                if (returnNext == true)
                {
                    break;
                }
            }
            getKey = entry.Key;

        }

        return get(getKey);
    }
}

[Serializable]
public class TileGameObjectDictionary
{
    [SerializeField]
    public TileGameObjectDictionaryItem[] tileGameObjectDictionaryItems;

    public Dictionary<string, SpriteAndGameObject> ToDictionary()
    {
        Dictionary<string, SpriteAndGameObject> newDict = new Dictionary<string, SpriteAndGameObject>();

        foreach (var item in tileGameObjectDictionaryItems)
        {
            newDict.Add(item.name, item.obj);
        }

        return newDict;
    }
}

[Serializable]
public class TileGameObjectDictionaryItem
{
    [SerializeField]
    public string name;
    [SerializeField]
    public SpriteAndGameObject obj;
}

[Serializable]
public class SpriteAndGameObject
{
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public GameObject obj;

}


