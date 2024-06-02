using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditorMouse : MonoBehaviour
{
    public enum LevelManipulation { Create, Rotate, Destroy };
    public TileGameObjects tileGameObjects;

    [HideInInspector]
    public SpriteAndGameObject selectedGameObject;
    public string selectedGameObjectName;
   // public GameObject stagingArea;
   // public GameObject stagingAreaBackground;
   // public GameObject stagingAreaForeground;

    

    [HideInInspector]
    public LevelManipulation manipulateOption = LevelManipulation.Create;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Material goodPlace;
    public Material badPlace;
    public GameObject Player;
    public LevelEditorManager levelEditorManager;
    private GameObject selectedStagingArea;
    public float grid_length = 128f;

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    public delegate void SelectedGameObjectChanged(string gameObjectName);
    public static event SelectedGameObjectChanged onSelectedGameObjectChanged;

    public static Vector3 mousePosition;
    public static GameObject selectedObject;

    private SnapToGrid snapToGrid;
    private bool isSnapToGrid = true;

    private Vector3 lastSetPosition;
    private bool mouseIsDown = false;

    private int objectCount = 0;

    Vector3 offset;

    private void OnEnable()
    {
        GridSizeSliderController.onSliderLoad += 
            (float initial_value) => { grid_length = initial_value; };
        LayerSwitchButtonController.onLayerSwitchUpdate += 
            (GameObjectPosition.LayerPosition layerPosition) => 
            onSelectedStagingAreaUpdate(layerPosition);
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setSelectedGameObject(tileGameObjects.get("Player"));
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

    public void setSelectedGameObject(SpriteAndGameObject o)
    {
        selectedGameObject = o; 
        selectedGameObjectName = o.obj.name;
        onSelectedGameObjectChanged.Invoke(selectedGameObjectName);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEditorManager.getEditorMode() == LevelEditorManager.EditorMode.PLAYMODE)
        {
            return;
        }
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
                if (selectedObject && selectedObject.transform.parent.transform == selectedStagingArea.transform)
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
            if (selectedObject && selectedObject.transform.parent.transform == selectedStagingArea.transform)
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

    private void onSelectedStagingAreaUpdate(GameObjectPosition.LayerPosition layerPosition)
    {
        switch (layerPosition)
        {
            case GameObjectPosition.LayerPosition.MIDDLEGROUND:
                selectedStagingArea = levelEditorManager.tileMapGenerator.stagingArea; break;
            case GameObjectPosition.LayerPosition.BACKGROUND:
                selectedStagingArea = levelEditorManager.tileMapGenerator.stagingAreaBackground; break;
            case GameObjectPosition.LayerPosition.FOREGROUND:
                selectedStagingArea = levelEditorManager.tileMapGenerator.stagingAreaForeground; break;
            default:
                selectedStagingArea = levelEditorManager.tileMapGenerator.stagingArea; break;
        }
    }

    void CreateObject()
    {
        objectCount += 1;
        GameObject newObj = new GameObject(selectedGameObject.obj.name);
        SpriteRenderer sr = newObj.AddComponent<SpriteRenderer>();
        LevelEditorMoveableObject moveable = newObj.AddComponent<LevelEditorMoveableObject>();
        
        Canvas canvas = newObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        GameObject childObj = new GameObject(selectedGameObject.obj.name + "Text");
        childObj.transform.parent = newObj.transform;
        childObj.transform.localScale = new Vector3(0.1f,0.1f,1f);
        ContentSizeFitter csf = childObj.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


        TextMeshProUGUI tm = childObj.AddComponent<TextMeshProUGUI>();
        tm.outlineColor = Color.black;
        tm.outlineWidth = 0.134f;
        tm.SetText(objectCount.ToString());
        tm.fontSize = 6;

        moveable.myRenderer = sr;
        
        sr.sprite = selectedGameObject.sprite;
        newObj.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedGameObject.obj.transform.position.z);
        newObj.transform.parent = selectedStagingArea.transform;
    }

    public void clearSelectedStageArea()
    {
        clearStagingArea(selectedStagingArea.transform);
    }

    private void clearStagingArea(Transform area)
    {
        foreach (Transform child in area)
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

    public void clearStagingAreas()
    {
        objectCount = 0;
        clearStagingArea(levelEditorManager.tileMapGenerator.stagingAreaBackground.transform);
        clearStagingArea(levelEditorManager.tileMapGenerator.stagingAreaForeground.transform);
        clearStagingArea(levelEditorManager.tileMapGenerator.stagingArea.transform);
    }

    private void OnDisable()
    {
        GridSizeSliderController.onSliderLoad -= 
            (float initial_value) => { grid_length = initial_value; };
        LayerSwitchButtonController.onLayerSwitchUpdate -=
            (GameObjectPosition.LayerPosition layerPosition) =>
            onSelectedStagingAreaUpdate(layerPosition);
    }
}
