using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Map/Room")]
public class SO_Room : ScriptableObject
{
    public string RoomName;
    public int SplitAmount;
    public int MergeAmount;
    public Vector2Int Size;
    public GameObject RoomPrefab;
    private static Dictionary<string, int> cloneCounters = new Dictionary<string, int>();

    public enum Direction { North, East, South, West }

    [System.Serializable]
    public class DirectionState
    {
        public Direction direction;
        public bool isActive;
    }

    [SerializeField]
    public List<DirectionState> directionStates = new List<DirectionState>();

    public Vector2 GetRandomEnabledDirection()
    {
        // Filter the enabled directions
        List<Direction> enabledDirections = new List<Direction>();
        foreach (var state in directionStates)
        {
            if (state.isActive)
            {
                enabledDirections.Add(state.direction);
            }
        }

        if (enabledDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, enabledDirections.Count);
            Direction randomDirection = enabledDirections[randomIndex];
            return GetDirectionOffset(randomDirection);
        }

        Debug.LogError($"Can not find enabled Direction {RoomName}");
        return Vector2.zero; // No enabled directions, return default
    }

    public void SetDirection(Vector2 directionVector, bool state)
    {
        Direction direction = Vector2ToDirection(directionVector);

        foreach (var dirState in directionStates)
        {
            if (dirState.direction == direction)
            {
                dirState.isActive = state;
                Debug.Log($"Set {direction} to {state}");
                return;
            }
        }
        Debug.LogError("Direction not found or state not set.");
        
    }

    private Direction Vector2ToDirection(Vector2 directionVector)
    {
        if (directionVector.x == 0 && directionVector.y > 0) return Direction.North;
        if (directionVector.x > 0 && directionVector.y == 0) return Direction.East;
        if (directionVector.x == 0 && directionVector.y < 0) return Direction.South;
        if (directionVector.x < 0 && directionVector.y == 0) return Direction.West;
        Debug.LogError($"Can no Identify{directionVector}");
        return Direction.North; // Default direction fallback
    }

    private Vector2 GetDirectionOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Vector2.up;
            case Direction.East:
                return Vector2.right;
            case Direction.South:
                return Vector2.down;
            case Direction.West:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
    public SO_Room Clone()
{
    // Create a new instance
    SO_Room clone = ScriptableObject.CreateInstance<SO_Room>();
    clone.Size = this.Size;
    clone.SplitAmount = this.SplitAmount;
    clone.RoomPrefab = RoomPrefab;
    
    // Copy directionStates
    clone.directionStates = new List<DirectionState>();
    foreach (var dirState in this.directionStates)
    {
        DirectionState clonedDirState = new DirectionState
        {
            direction = dirState.direction,
            isActive = dirState.isActive
        };
        clone.directionStates.Add(clonedDirState);
    }

    // Update the name with a unique identifier
    if (!cloneCounters.ContainsKey(RoomName))
    {
        cloneCounters[RoomName] = 0;
    }
    cloneCounters[RoomName]++;
    clone.RoomName = $"{RoomName}_Clone{cloneCounters[RoomName]}";
    clone.name = clone.RoomName;

    return clone;
}
}
