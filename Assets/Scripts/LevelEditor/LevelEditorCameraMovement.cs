using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCameraMovement : MonoBehaviour
{
    public Camera m_Camera;
    public float scale = 1f;
    private Vector3 camera_initial_position;

    private void Start()
    {
        camera_initial_position = m_Camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       m_Camera.orthographicSize = m_Camera.orthographicSize + Input.mouseScrollDelta.y * scale;

       if (Input.GetKey(KeyCode.Space))
       {
            Vector3 offset = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_Camera.transform.position) * 0.01f;
            m_Camera.transform.position = new Vector3(
                m_Camera.transform.position.x + offset.x, 
                m_Camera.transform.position.y + offset.y, 
                m_Camera.transform.position.z
            );
       }

    }
}
