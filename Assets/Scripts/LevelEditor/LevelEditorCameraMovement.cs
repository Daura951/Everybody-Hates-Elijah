using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEditorCameraMovement : MonoBehaviour
{
    public Camera m_Camera;
    public float scale = 1f;
    private Vector3 camera_initial_position;

    public delegate void ZoomLevelChanged(float zoomLevel);
    public static event ZoomLevelChanged onZoomLevelChanged;

    private void Start()
    {
        camera_initial_position = m_Camera.transform.position;
        onZoomLevelChanged?.Invoke(m_Camera.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.mouseScrollDelta.y != 0)
       {
            m_Camera.orthographicSize = m_Camera.orthographicSize + Input.mouseScrollDelta.y * scale;
            onZoomLevelChanged?.Invoke(m_Camera.orthographicSize);
        }

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
