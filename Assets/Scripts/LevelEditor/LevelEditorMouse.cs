using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditorMouse : MonoBehaviour
{

    [SerializeField]
    private Texture2D pointer_texture;
    [SerializeField]
    private Texture2D paint_brush_texture;
    [SerializeField]
    private Texture2D eraser_texture;
    [SerializeField]
    private Texture2D move_texture;

    enum cursor_texture_mode_type { pointer, brush, eraser, move }
    private cursor_texture_mode_type cursor_texture_mode = cursor_texture_mode_type.pointer;

    public enum LevelManipulation { Create, Rotate, Destroy };
    public TileGameObjects tileGameObjects;

    [HideInInspector]
    public SpriteAndGameObject selectedGameObject;
    public string selectedGameObjectName;

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
    public GameObject selectedObject;

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
        EditModeButton.onLevelManipulationChange += (LevelManipulation lm) => updateLevelManipulation(lm);
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setSelectedGameObject(tileGameObjects.get("Player"));
        snapToGrid = new SnapToGrid();

    }

    private void Start()
    {
        //Cursor.visible = false;
        Cursor.SetCursor(pointer_texture, Vector2.zero, CursorMode.ForceSoftware);
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

        Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            mouseIsDown = true;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                mouseIsDown = false;
            }

            if (targetObject && targetObject?.transform.transform.parent.transform == selectedStagingArea.transform)
            {
                selectedObject = targetObject.transform.gameObject;
            }
            if (selectedObject && selectedObject.transform.parent.transform == selectedStagingArea.transform)
            {
                offset = selectedObject.transform.position - mousePosition;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
            mouseIsDown = false;
        }

        // TODO: Refactor and make a state machine with transitions instead of this if statement hell
        if (targetObject && targetObject.transform.parent.transform == selectedStagingArea.transform && manipulateOption != LevelManipulation.Destroy)
        {
            if (cursor_texture_mode != cursor_texture_mode_type.move)
            {
                Cursor.SetCursor(move_texture, Vector2.zero, CursorMode.ForceSoftware);
                cursor_texture_mode = cursor_texture_mode_type.move;
            }
        } else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (cursor_texture_mode != cursor_texture_mode_type.pointer)
                {
                    Cursor.SetCursor(pointer_texture, Vector2.zero, CursorMode.ForceSoftware);
                    cursor_texture_mode = cursor_texture_mode_type.pointer;
                }
            } else
            {
                if (manipulateOption == LevelManipulation.Create)
                {
                    if (cursor_texture_mode != cursor_texture_mode_type.brush)
                    {
                        Cursor.SetCursor(paint_brush_texture, Vector2.zero, CursorMode.ForceSoftware);
                        cursor_texture_mode = cursor_texture_mode_type.brush;
                    }
                }
                if (manipulateOption == LevelManipulation.Destroy)
                {
                    if (cursor_texture_mode != cursor_texture_mode_type.eraser)
                    {
                        Cursor.SetCursor(eraser_texture, Vector2.zero, CursorMode.ForceSoftware);
                        cursor_texture_mode = cursor_texture_mode_type.eraser;
                    }
                }
            }
        }

        if (mouseIsDown)
        {
            if (manipulateOption == LevelManipulation.Destroy)
            {
                if (targetObject)
                {
                    Destroy(targetObject.transform.gameObject);
                }
            }

            if (manipulateOption == LevelManipulation.Create)
            {
                if (
                    !selectedObject && 
                    targetObject?.transform.transform.parent.transform != selectedStagingArea.transform &&
                    lastSetPosition != mousePosition &&
                    manipulateOption == LevelManipulation.Create

                )
                { 
                    CreateObject();
                    lastSetPosition = mousePosition;
                }
                if (selectedObject && selectedObject.transform.parent.transform == selectedStagingArea.transform)
                {
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

        BoxCollider2D collider = newObj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1.28f, 1.28f);

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
        tm.outlineWidth = 0.14f;
        tm.SetText(objectCount.ToString());
        tm.fontSize = 5;
        
        sr.sprite = selectedGameObject.sprite;
        newObj.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedGameObject.obj.transform.position.z);
        newObj.transform.parent = selectedStagingArea.transform;
    }

    public void clearSelectedStageArea()
    {
        clearStagingArea(selectedStagingArea.transform);
    }

    private void updateLevelManipulation(LevelManipulation lm)
    {
        manipulateOption = lm;

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
        EditModeButton.onLevelManipulationChange -= (LevelManipulation lm) => updateLevelManipulation(lm);

    }
}
