using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    public SO_Room RoomData;
    public Vector2Int GridPosition;
    private BoxCollider triggerCollider;

    public void SetupRoom(SO_Room roomData, Vector2Int position)
    {
        RoomData = roomData;
        GridPosition = position;
        transform.position = new Vector3(position.x, 0, position.y);

        triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector3(RoomData.Size.x, 2f, RoomData.Size.y);
        triggerCollider.center = new Vector3(0, 1f, 0);
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