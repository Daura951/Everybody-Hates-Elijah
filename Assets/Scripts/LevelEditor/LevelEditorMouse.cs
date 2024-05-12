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
    private RaycastHit hit;

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    public static Vector3 mousePosition;
    public static GameObject selectedObject;

    Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedGameObject = tileGameObjects.get("Player");
        selectedGameObjectName = "Player";
    }

    // Update is called once per frame
     void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        { 
            if (OnMouseClick != null)
            {
                OnMouseClick();

                if (selectedObject)
                {

                    if (manipulateOption == LevelManipulation.Destroy)
                    {
                        Destroy(selectedObject.gameObject);
                    } else
                    {
                        offset = selectedObject.transform.position - mousePosition;
                    }
                    
                }
  
            }

           if (
                manipulateOption == LevelManipulation.Create && 
                !selectedObject && 
                !EventSystem.current.IsPointerOverGameObject()
            )
            {
                CreateObject();
            }

        }

        if (selectedObject) // subscriber tells if it is selected or not
        {
            Debug.Log("setting position");
            selectedObject.transform.position = mousePosition + offset;
        }


        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
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
        Debug.Log("Creating object : " + selectedGameObject.obj.name);
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
}
