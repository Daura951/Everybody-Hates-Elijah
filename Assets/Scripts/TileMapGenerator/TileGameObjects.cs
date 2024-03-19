using System;
using System.Collections;
using System.Collections.Generic;
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

