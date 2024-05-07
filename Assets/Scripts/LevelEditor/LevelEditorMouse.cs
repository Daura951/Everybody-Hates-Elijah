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
    private Vector3 mousePos;
    private bool colliding;
    private Ray ray;
    private RaycastHit hit;

    Vector3 offset;

    public GameObject selectedObject;

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
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
  
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

                if (targetObject)
                {
                    selectedObject = targetObject.transform.gameObject;
                    offset = selectedObject.transform.position - mousePosition;
                }

            if (!EventSystem.current.IsPointerOverGameObject() && !targetObject)
            {
                if (colliding == false && manipulateOption == LevelManipulation.Create)
                {
                    CreateObject();
                }
                else if (colliding == true && manipulateOption == LevelManipulation.Destroy)
                {
                    if (hit.collider.gameObject.name.Contains("Player"))
                    {
                        levelEditorManager.playerPlaced = false;
                    }
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        if (selectedObject)
        {
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
        sr.sprite = selectedGameObject.sprite;
        newObj.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        newObj.transform.parent = stagingArea.transform;
        Debug.Log("Creating object : " + selectedGameObject.obj.name);
    }

    public void clearStagingArea()
    {
        foreach (Transform child in stagingArea.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
