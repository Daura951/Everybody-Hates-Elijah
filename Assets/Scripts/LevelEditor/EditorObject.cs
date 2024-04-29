using System;
using UnityEngine;
public class EditorObject : MonoBehaviour
{
    public string ObjectType;
    [Serializable]
    public struct Data
    {
        public Vector3 pos;
        public Quaternion rot;
        public string objectType;
    }
    public Data data;
}
