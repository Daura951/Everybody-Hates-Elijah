using UnityEngine;

public class Door_Managed : MonoBehaviour
{
    private GameObject connectedDoor;

    public delegate void OnDoorCreated(Door_Managed door);
    public static OnDoorCreated onDoorCreated;

    void Start()
    {
        // Send "I'm here" event with self,
        // DoorManager is the listener
        // (could be keys, or switches or anything else too!)
        onDoorCreated?.Invoke(this);
    }

    // used by DoorManager when setting up door links
    public void SetConnectedDoor(GameObject go)
    {
        connectedDoor = go;
    }

    public void SendObjectToLinkedDoor(GameObject go)
    {
        go.transform.position = new Vector3(
            connectedDoor.transform.position.x,
            connectedDoor.transform.position.y,
            go.transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObjectToDoorInterface gameObjectToDoorInterface = col.gameObject.GetComponent<GameObjectToDoorInterface>();
        if (gameObjectToDoorInterface == null)
        {
            return;
        }
        gameObjectToDoorInterface.setDoorManaged(this);
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        GameObjectToDoorInterface gameObjectToDoorInterface = col.gameObject.GetComponent<GameObjectToDoorInterface>();
        if (gameObjectToDoorInterface == null)
        {
            return;
        }
        gameObjectToDoorInterface.removeDoorManaged();
    }
}
