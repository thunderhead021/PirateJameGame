using UnityEngine;

public static class DebugRenderer
{
    public static void DrawRoom(Vector2Int position, Vector2Int size, string roomName)
    {
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x, 0, position.y + size.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x + size.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 10f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y + size.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 10f);
    }

    public static void DrawPath(Vector2Int start, Vector2Int end)
    {
        Debug.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y), Color.red, 10f);
    }

    public static void ClearDebug()
    {
    }
     public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration)
    {
        Debug.DrawLine(position - Vector3.up * radius, position + Vector3.up * radius, color, duration);
        Debug.DrawLine(position - Vector3.right * radius, position + Vector3.right * radius, color, duration);
        Debug.DrawLine(position - Vector3.forward * radius, position + Vector3.forward * radius, color, duration);
    }
}