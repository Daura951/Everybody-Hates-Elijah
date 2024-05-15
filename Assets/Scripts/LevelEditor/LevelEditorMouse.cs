using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorMouse : MonoBehaviour
{
    public enum LevelManipulation { Create, Rotate, Destroy };
    public TileGameObjects tileGameObjects;

    [HideInInspector]
    public SpriteAndGameObject selectedGameObject;
    public string selectedGameObjectName;
    public GameObject stagingArea;

    [HideInInspector]
    public LevelManipulation manipulateOption = LevelManipulation.Create;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Material goodPlace;
    public Material badPlace;
    public GameObject Player;
    public LevelEditorManager levelEditorManager;
    public float grid_length = 128f;

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    public static Vector3 mousePosition;
    public static GameObject selectedObject;

    private SnapToGrid snapToGrid;
    private bool isSnapToGrid = true;

    private Vector3 lastSetPosition;
    private bool mouseIsDown = false;

    Vector3 offset;

    private void OnEnable()
    {
        GridSizeSliderController.onSliderLoad += (float initial_value) => { grid_length = initial_value; };
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedGameObject = tileGameObjects.get("Player");
        selectedGameObjectName = "Player";
        snapToGrid = new SnapToGrid();
    }

    public void toggleIsSnapToGrid()
    {
        isSnapToGrid = !isSnapToGrid;
    }

    public void onGridSizeSliderChange (float newValue)
    {
        grid_length = newValue;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePositionReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float snapped_mouse_x = snapToGrid.snap(mousePositionReal.x, grid_length);
        float snapped_mouse_y = snapToGrid.snap(mousePositionReal.y, grid_length);

        mousePosition = new Vector2(
            isSnapToGrid ? snapped_mouse_x : mousePositionReal.x,
            isSnapToGrid ? snapped_mouse_y : mousePositionReal.y
            );

        if (Input.GetMouseButtonDown(0))
        {
            mouseIsDown = true;
            if (OnMouseClick != null)
            {
                OnMouseClick();
                if (selectedObject)
                {
                    offset = selectedObject.transform.position - mousePosition;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject)
            {
                selectedObject = null;
            }
            mouseIsDown = false;
        }

        if (mouseIsDown)
        {
            if (
                !selectedObject &&
                lastSetPosition != mousePosition &&
                manipulateOption == LevelManipulation.Create &&
                !EventSystem.current.IsPointerOverGameObject())
            { 
                CreateObject();
                lastSetPosition = mousePosition;
            }
            if (selectedObject)
            {
                if (manipulateOption == LevelManipulation.Destroy)
                {
                    Destroy(selectedObject.gameObject);
                } else {
                    selectedObject.transform.position = mousePosition + offset;
                }
            }
        }
    }

    void CreateObject()
    {
        GameObject newObj = new GameObject(selectedGameObject.obj.name);
        SpriteRenderer sr = newObj.AddComponent<SpriteRenderer>();
        LevelEditorMoveableObject moveable = newObj.AddComponent<LevelEditorMoveableObject>();
        moveable.myRenderer = sr;
        
        sr.sprite = selectedGameObject.sprite;
        newObj.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        newObj.transform.parent = stagingArea.transform;
    }

    public void clearStagingArea()
    {
        foreach (Transform child in stagingArea.transform)
        {
            Destroy(child.gameObject);
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam.gameObject.name != "Main Camera")
                {
                    Destroy(cam.gameObject);
                }
                else
                {
                    cam.transform.position = new Vector3(0f, 0f, -20f);
                }
            }
        }

    }

    private void OnDisable()
    {
        GridSizeSliderController.onSliderLoad -= (float initial_value) => { grid_length = initial_value; };
    }
}
