using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorMouse : MonoBehaviour
{
    public enum LevelManipulation { Create, Rotate, Destroy };
    public TileGameObjects tileGameObjects;

    [HideInInspector]
    public GameObject selectedGameObject;
    public string selectedGameObjectName;
    public GameObject stagingArea;

    [HideInInspector]
    public LevelManipulation manipulateOption = LevelManipulation.Create;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public GameObject rotObject;
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
        selectedGameObject = tileGameObjects.getGameObject("Player");
        selectedGameObjectName = "player";
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
                if (colliding == false && manipulateOption ==
                                LevelManipulation.Create)
                    CreateObject();
                else if (colliding == true && manipulateOption ==
                                LevelManipulation.Rotate)
                    SetRotateObject();
                else if (colliding == true && manipulateOption ==
                                LevelManipulation.Destroy)
                {
                    if (hit.collider.gameObject.name.Contains(
                                         "Player"))
                        levelEditorManager.playerPlaced = false;
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
        GameObject newObj;
        newObj = selectedGameObject;
        newObj.transform.position = transform.position;
        //EditorObject eo = newObj.AddComponent<EditorObject>();
        //eo.data.pos = newObj.transform.position;
        //eo.data.rot = newObj.transform.rotation;
        //eo.data.objectType = selectedGameObjectName;
        Instantiate(newObj, new Vector3(mousePos.x, mousePos.y, 0f), Quaternion.identity, stagingArea.transform);
        Debug.Log("Creating object : " + selectedGameObject.name);
    }

    void SetRotateObject()
    {
        rotObject = hit.collider.gameObject;
        levelEditorManager.rotSlider.value = rotObject.transform.rotation.y;
    }
}
