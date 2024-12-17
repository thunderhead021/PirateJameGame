using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    public SO_Room RoomData;
    private Collider triggerCollider;

    public void SetupRoom(SO_Room roomData, Vector2Int position)
    {
        RoomData = roomData;
        triggerCollider = this.GetComponentInChildren<Collider>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{RoomData.RoomName} entered.");
            HandleRoomEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{RoomData.RoomName} Exited.");
            HandleRoomExit();
        }
    }

    private void HandleRoomEnter()
    {
        Debug.Log($"Entering Logic for {RoomData.RoomName} executed.");
    }
    private void HandleRoomExit()
    {
        Debug.Log($"Leaving Logic for {RoomData.RoomName} executed.");
    }
}