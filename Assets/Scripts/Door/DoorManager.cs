using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // private List<Door_Managed> doors_managed = new List<Door_Managed>();
    private Door_Managed previousDoor;

    private void OnEnable()
    {
        Door_Managed.onDoorCreated += (Door_Managed dm) => handleDoor(dm);
    }

    private void handleDoor(Door_Managed dm)
    {
        // This pattern lets us connect every pair of doors
        // if there is an odd number the last will not be connected
        if (previousDoor != null)
        {
            dm.SetConnectedDoor(previousDoor.gameObject);
            previousDoor.SetConnectedDoor(dm.gameObject);
            previousDoor = null;
            return;
        }
        
        previousDoor = dm;
    }

    private void OnDisable()
    {
        Door_Managed.onDoorCreated -= (Door_Managed dm) => handleDoor(dm);
    }
}
