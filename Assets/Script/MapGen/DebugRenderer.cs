using System.Collections.Generic;
using UnityEngine;

public static class DebugRenderer
{
    private static List<DebugLine> debugLines = new List<DebugLine>();

    public static void DrawRoom(Vector2Int position, Vector2Int size, string roomName)
    {
        AddDebugLine(new Vector3(position.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y));
        AddDebugLine(new Vector3(position.x, 0, position.y), new Vector3(position.x, 0, position.y + size.y));
        AddDebugLine(new Vector3(position.x + size.x, 0, position.y), new Vector3(position.x + size.x, 0, position.y + size.y));
        AddDebugLine(new Vector3(position.x, 0, position.y + size.y), new Vector3(position.x + size.x, 0, position.y + size.y));
    }

    public static void DrawPath(Vector3 start, Vector3 end)
    {
        Debug.Log($"Drawing Path from {start} to {end}");
        AddDebugLine(new Vector3(start.x, 1, start.z), new Vector3(end.x, 1, end.z));
    }

    public static void ClearDebug()
    {
        debugLines.Clear();
    }

    private static void AddDebugLine(Vector3 start, Vector3 end)
    {
        debugLines.Add(new DebugLine(start, end, Color.green, 60f));
    }

    public static void DrawDebugSphere(Vector3 position, float radius, Color color)
    {
        AddDebugLine(position - Vector3.up * radius, position + Vector3.up * radius, color);
        AddDebugLine(position - Vector3.right * radius, position + Vector3.right * radius, color);
        AddDebugLine(position - Vector3.forward * radius, position + Vector3.forward * radius, color);
    }

    private static void AddDebugLine(Vector3 start, Vector3 end, Color color)
    {
        debugLines.Add(new DebugLine(start, end, color, 60f));
    }

    private class DebugLine
    {
        public Vector3 Start { get; }
        public Vector3 End { get; }
        public Color Color { get; }
        public float Duration { get; }

        public DebugLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            Start = start;
            End = end;
            Color = color;
            Duration = duration;
            Debug.DrawLine(start, end, color, duration);
        }
    }
}
