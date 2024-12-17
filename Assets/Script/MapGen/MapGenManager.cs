using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    public List<SO_Room> RoomList;
    public SO_Room spawnRoom;
    public SO_Room bossRoom;
    public List<SO_LevelInstance> LevelInstances;
    public int CurrentLevelIndex = 0;
    public int Seed;
    public int MinIntermediaryRooms = 1;
    public int MaxIntermediaryRooms = 3;
    public int RoomRandomnessMin = 2;
    public int RoomRandomnessMax = 5;
    public bool ShowDebug = true;
    public bool generateMap = false;

    private struct RoomPlacement
    {
        public Vector2Int Position;
        public Vector2Int Size;
        public SO_Room roomData;

        public RoomPlacement(Vector2Int position, Vector2Int size, SO_Room data)
        {
            Position = position;
            Size = size;
            roomData = data;
        }
    }

    private List<RoomPlacement> occupiedPositions = new List<RoomPlacement>();

    public void Update()
    {
        if (generateMap)
        {
            LoadLevel(CurrentLevelIndex);
            Debug.Log("Generating Map");
            generateMap = false;
        }
    }

    public void GenerateMap()
    {
        ClearMap();
        Random.InitState(Seed);

        Vector2Int spawnPosition = Vector2Int.zero;
        occupiedPositions.Clear();

        // Place the Spawn Room
        PlaceRoomExact(spawnRoom, spawnPosition);

        // Place the Boss Room
        SO_LevelInstance currentLevel = LevelInstances[CurrentLevelIndex];
        Vector2Int bossRoomPosition = new Vector2Int(0, currentLevel.LevelDistance);
        PlaceRoomExact(bossRoom, bossRoomPosition);

        // Place intermediary rooms
        foreach (var room in RoomList)
        {
            PlaceRoom(room, spawnPosition);
        }

        // Generate paths between rooms
        GeneratePaths();
    }

    private void PlaceRoomExact(SO_Room roomData, Vector2Int position)
    {
        occupiedPositions.Add(new RoomPlacement(position, roomData.Size, roomData));

        GameObject roomObject = new GameObject(roomData.RoomName);
        roomObject.transform.position = new Vector3(position.x, 0, position.y);
        RoomInstance instance = roomObject.AddComponent<RoomInstance>();
        instance.SetupRoom(roomData, position);

        if (ShowDebug)
        {
            DebugRenderer.DrawRoom(position, roomData.Size, roomData.RoomName);
        }
    }

    private Vector2Int PlaceRoom(SO_Room roomData, Vector2Int spawnPosition)
    {
        SO_LevelInstance currentLevel = LevelInstances[CurrentLevelIndex];
        Vector2Int bossPosition = new Vector2Int(0, currentLevel.LevelDistance);

        int totalDistance = Mathf.Abs(spawnPosition.y - bossPosition.y);
        int roomIndex = occupiedPositions.Count - 1;
        float t = (float)roomIndex / (RoomList.Count + 1);
        int positionY = Mathf.RoundToInt(totalDistance * t);

        Vector2Int position = new Vector2Int(
            Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1),
            positionY + Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1)
        );

        if (CheckOverlap(position, roomData.Size))
        {
            return spawnPosition; // Return spawn position if overlap detected
        }

        occupiedPositions.Add(new RoomPlacement(position, roomData.Size, roomData));

        GameObject roomObject = new GameObject(roomData.RoomName);
        roomObject.transform.position = new Vector3(position.x, 0, position.y);
        RoomInstance instance = roomObject.AddComponent<RoomInstance>();
        instance.SetupRoom(roomData, position);

        if (ShowDebug)
        {
            DebugRenderer.DrawRoom(position, roomData.Size, roomData.RoomName);
        }

        return position;
    }

    private bool CheckOverlap(Vector2Int position, Vector2Int size)
    {
        foreach (RoomPlacement placement in occupiedPositions)
        {
            if (position.x < placement.Position.x + placement.Size.x + RoomRandomnessMin &&
                position.x + size.x + RoomRandomnessMin > placement.Position.x &&
                position.y < placement.Position.y + placement.Size.y + RoomRandomnessMin &&
                position.y + size.y + RoomRandomnessMin > placement.Position.y)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadLevel(int levelIndex)
{
    if (levelIndex < 0 || levelIndex >= LevelInstances.Count)
    {
        Debug.LogError("Invalid level index");
        return;
    }

    SO_LevelInstance level = LevelInstances[levelIndex];
    CurrentLevelIndex = levelIndex;

    // Clone each room from the level's RoomList and add it to RoomList
    spawnRoom = spawnRoom.Clone();
    bossRoom = bossRoom.Clone();
    RoomList = new List<SO_Room>();
    foreach (var room in level.RoomList)
    {
        RoomList.Add(room.Clone());
    }

    Seed = level.Seed;
    MinIntermediaryRooms = level.MinIntermediaryRooms;
    MaxIntermediaryRooms = level.MaxIntermediaryRooms;
    RoomRandomnessMin = level.RoomRandomnessMin;
    RoomRandomnessMax = level.RoomRandomnessMax;

    GenerateMap();
}

    public void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        DebugRenderer.ClearDebug();
    }

    public void GeneratePaths()
    {
        foreach (var roomPlacement in occupiedPositions)
        {
            for (int i = 0; i < roomPlacement.roomData.SplitAmount; i++)
            {
                GeneratePathForRoom(roomPlacement);
                StartCoroutine(DelayBeforeNextPath(2.0f)); // 2-second delay
            }
        }
    }
    
    private IEnumerator DelayBeforeNextPath(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
    }
    
    private void GeneratePathForRoom(RoomPlacement startRoom)
    {
        // Get a random enabled direction as Vector2 offset
        Vector2? directionOffset = startRoom.roomData.GetRandomEnabledDirection();
        
        if (directionOffset.HasValue)
        {
            Vector2 position = startRoom.Position + directionOffset.Value;

            // Spawn a debug sphere at the midpoint of the edge in the chosen direction
            if (ShowDebug)
            {
                DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.red, 5f); // Red sphere with radius 0.5 and duration of 5 seconds
            }

            // Convert the Vector2 direction back to Direction enum and set it as inactive
            startRoom.roomData.SetDirection(directionOffset.Value, false);
        }
        else
        {
            Debug.LogWarning("No enabled direction found for room: " + startRoom.roomData.RoomName);
        }
    }
 
}
