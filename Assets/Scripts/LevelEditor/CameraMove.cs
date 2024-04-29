using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public Slider cameraSpeedSlide;
    public LevelEditorManager levelEditorManager;

    private float xAxis;
    private float yAxis;
    private float zoom;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEditorManager.saveLoadMenuOpen == false)
        {
            xAxis = Input.GetAxis("Horizontal");
            yAxis = Input.GetAxis("Vertical");
            zoom = Input.GetAxis("Mouse ScrollWheel") * 10;

            transform.Translate(new Vector3(xAxis * -cameraSpeedSlide.value, yAxis * -cameraSpeedSlide.value, 0.0f));
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -20, 20),
                Mathf.Clamp(transform.position.y, 20, 20),
                Mathf.Clamp(transform.position.z, -20, 20)
            );
            if (zoom < 0 && cam.orthographicSize >= -25)
            {
                cam.orthographicSize -= zoom * -cameraSpeedSlide.value;
            }
            if (zoom > 0 && cam.orthographicSize <= -5)
            {
                cam.orthographicSize += zoom * cameraSpeedSlide.value;
            }
        }

    }
}
