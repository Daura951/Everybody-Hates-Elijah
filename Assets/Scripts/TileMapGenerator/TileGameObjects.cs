using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class TileGameObjects : MonoBehaviour
{
    [SerializeField]
    string thisObjectName;

    [SerializeField]
    public TileGameObjectDictionary newDict;

    private Dictionary<string, GameObject> tileGameObjectDictionary;

    private void Awake()
    {
        tileGameObjectDictionary = newDict.ToDictionary();
    }

    public GameObject getGameObject(string key)
    {
        if (!tileGameObjectDictionary.ContainsKey(key))
        {
            Debug.LogError("Missing '" + key + "' tile in the tileGameObjects dictionary.");
        }
        return tileGameObjectDictionary[key];
    }

    public GameObject getNextGameObject(string key)
    {
        string firstKey = "";
        string getKey = "";
        bool returnNext = false;
        foreach (KeyValuePair<string, GameObject> entry in tileGameObjectDictionary)
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
        return getGameObject(getKey);
    }
    public GameObject getPreviousGameObject(string key)
    {
        bool getLast = false;
        string firstKey = "";
        string getKey = "";
        bool returnNext = false;
        foreach (KeyValuePair<string, GameObject> entry in tileGameObjectDictionary)
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

        return getGameObject(getKey);
    }
}

[Serializable]
public class TileGameObjectDictionary
{
    [SerializeField]
    public TileGameObjectDictionaryItem[] tileGameObjectDictionaryItems;

    public Dictionary<string, GameObject> ToDictionary()
    {
        Dictionary<string, GameObject> newDict = new Dictionary<string, GameObject>();

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
    public GameObject obj;
}


