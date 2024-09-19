using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class FloatGameObjectPair
{
    public float time;
    public GameObject uiObject;
}

public class RankUIManager : MonoBehaviour
{
    private Dictionary<float, GameObject> timerToObject;
    public List<FloatGameObjectPair> timerToObjectList;
    private List<float> times = new List<float>();
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        timerToObject = timerToObjectList.ToDictionary(pair => pair.time, pair => pair.uiObject);
        times = timerToObject.Keys.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        foreach(float time in times)
        {
            if(timer >= time)
            {
                timerToObject.GetValueOrDefault(time).SetActive(true);
            }
        }
    }
}
