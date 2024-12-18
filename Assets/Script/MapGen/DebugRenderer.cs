using UnityEngine;

public static class DebugRenderer
{
    public static void DrawRoom(Vector2Int position, Vector2Int size, string roomName)
    {
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y), Color.green, 60f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y), new Vector3(position.x, 0, position.y + size.y), Color.green, 60f);
        Debug.DrawLine(new Vector3(position.x + size.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 60f);
        Debug.DrawLine(new Vector3(position.x, 0, position.y + size.y), new Vector3(position.x + size.x, 0, position.y + size.y), Color.green, 60f);
    }

    public static void DrawPath(Vector3 start, Vector3 end)
    {
        Debug.Log($"DrawingPath{start} to {end}");
        Debug.DrawLine(new Vector3(start.x, 1, start.z), new Vector3(end.x, 1, end.z), Color.green, 60f);
    }

    public static void ClearDebug()
    {
    }
    public static void DrawDebugSphere(Vector3 position, float radius, Color color)
    {
        Debug.DrawLine(position - Vector3.up * radius, position + Vector3.up * radius, color, 60f);
        Debug.DrawLine(position - Vector3.right * radius, position + Vector3.right * radius, color, 60f);
        Debug.DrawLine(position - Vector3.forward * radius, position + Vector3.forward * radius, color, 60f);
    }
}