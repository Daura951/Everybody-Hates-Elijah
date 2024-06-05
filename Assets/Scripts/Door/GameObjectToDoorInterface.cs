using UnityEngine;

// Used for anything that needs to go through doors
// Example: Players, Enemies, projectiles, etc.
public class GameObjectToDoorInterface : MonoBehaviour
{
    Door_Managed door_managed;

    public void setDoorManaged(Door_Managed dm)
    {
        door_managed = dm;
    }
    public void removeDoorManaged()
    {
        door_managed = null;
    }

    // call this method to send game object through door
    public void goThroughDoor()
    {
        if (door_managed == null)
        {
            return;
        }
        door_managed.SendObjectToLinkedDoor(gameObject);
    }
}
