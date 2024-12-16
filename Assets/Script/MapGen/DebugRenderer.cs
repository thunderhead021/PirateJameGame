using UnityEngine;

public static class DebugRenderer
{
    public static void DrawRoom(Vector2Int position, Vector2Int size, string roomName)
    {
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x, 0, position.y + size.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x + size.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y + size.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 10f);
        Debug.Log($"Room: {roomName} at {position}");
    }

    public static void DrawPath(Vector2Int start, Vector2Int end)
    {
        Debug.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y), Color.red, 10f);
    }

    public static void ClearDebug()
    {
    }
}