using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public List<SO_Room> RoomList;
    public SO_Room spawnRoom;
    public SO_Room bossRoom;
    public SO_Room hallwayRoom;
    public List<SO_LevelInstance> LevelInstances;
    public int CurrentLevelIndex = 0;
    public int Seed;
    public int MinIntermediaryRooms = 1;
    public int MaxIntermediaryRooms = 3;
    public int RoomRandomnessMin = 2;
    public int RoomRandomnessMax = 5;
    public bool ShowDebug = true;
    public bool generateMap = false;
    private List<Vector2Int> exits;
    private List<Vector2Int> entrys;
    private Vector3 bossEntry;
    private float pathRandom = 0.5f;

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

        exits = new List<Vector2Int>();
        entrys = new List<Vector2Int>();
        bossEntry = Vector3.zero;

        Vector2Int spawnPosition = Vector2Int.zero;
        occupiedPositions.Clear();

        PlaceRoomExact(spawnRoom, spawnPosition);

        SO_LevelInstance currentLevel = LevelInstances[CurrentLevelIndex];
        Vector2Int bossRoomPosition = new Vector2Int(0, currentLevel.LevelDistance);
        PlaceRoomExact(bossRoom, bossRoomPosition);

        foreach (var room in RoomList)
        {
            PlaceRoom(room, spawnPosition);
        }

        GeneratePoints();
    }

    private void InstRoom(SO_Room roomData, Vector2Int position)
    {
        GameObject roomObject = Instantiate(roomData.RoomPrefab);
        roomObject.name = roomData.RoomName;
        roomObject.transform.position = new Vector3(position.x, 0, position.y);
        RoomInstance instance = roomObject.AddComponent<RoomInstance>();
        instance.SetupRoom(roomData, position);
        occupiedPositions.Add(new RoomPlacement(position, roomData.Size, roomData));  
    }
    private void PlaceRoomExact(SO_Room roomData, Vector2Int position)
    {
        InstRoom(roomData, position);
    } 
    private void PlaceRoom(SO_Room roomData, Vector2Int spawnPosition)
    {
        SO_LevelInstance currentLevel = LevelInstances[CurrentLevelIndex];
        Vector2Int bossPosition = new Vector2Int(0, currentLevel.LevelDistance);

        int totalDistance = Mathf.Abs(spawnPosition.y - bossPosition.y);
        int roomIndex = occupiedPositions.Count - 1;
        float t = (float)roomIndex / (RoomList.Count + 1);
        int positionY = Mathf.RoundToInt(totalDistance * t);

        Vector2Int position = new Vector2Int(Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1), positionY + Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1));
        position.x = Mathf.RoundToInt(position.x / 10) * 10;  // Clamp X to multiples of 10
        position.y = Mathf.RoundToInt(position.y / 10) * 10; // Clamp Y to multiples of 10


        if (CheckOverlap(position, roomData.Size))
        {
            return;
        }

        InstRoom(roomData, position);
    }
    private bool CheckOverlap(Vector2Int position, Vector2Int size)
    {
        foreach (RoomPlacement placement in occupiedPositions)
        {
            // Check for overlap
            if (position.x < placement.Position.x + placement.Size.x &&
                position.x + size.x > placement.Position.x &&
                position.y < placement.Position.y + placement.Size.y &&
                position.y + size.y > placement.Position.y)
            {
                return true; // Overlaps
            }
        }
        return false; // No overlap
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
    public void GeneratePoints()
    {
        foreach (var roomPlacement in occupiedPositions)
        {
            for (int i = 0; i < roomPlacement.roomData.SplitAmount; i++)
            {
                GenerateExitForRoom(roomPlacement);
            }
            for (int i = 0; i < roomPlacement.roomData.MergeAmount; i++)
            {
                GenerateEntryForRoom(roomPlacement);
            }
        }
        FindPoints();
    }
  
    private void FindPoints()
    {
        Debug.Log($"finished Generating entrys {entrys.Count} exits{exits.Count} boss {bossEntry}");

        foreach (var exit in exits)
        {
            Vector2Int? closestEntry = null;
            float closestDistance = float.MaxValue;

            foreach (var entry in entrys)
            {
                float distance = Vector2.Distance(exit, entry);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEntry = entry;
                }
            }

            if (closestEntry.HasValue)
            {
                Debug.Log($"Closest entry to exit at {exit} is at {closestEntry.Value} with distance {closestDistance}");

                entrys.Remove(closestEntry.Value);
            }
            else
            {
                Debug.LogWarning($"No entry found for exit at {exit}");
            }
        }
    }
    private void GenerateExitForRoom(RoomPlacement startRoom)
    {
        Vector2? directionOffset = startRoom.roomData.GetRandomEnabledDirection();

        if (directionOffset.HasValue)
        {
            Vector2 position = startRoom.Position + (directionOffset.Value * (startRoom.roomData.Size.x / 2));
            Vector2Int positionInt = new Vector2Int((int)position.x, (int)position.y);
            if (ShowDebug)
            {
                DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.red);
            }
            exits.Add(positionInt);
            startRoom.roomData.SetDirection(directionOffset.Value, false);
        }
        else
        {
            Debug.LogWarning("No enabled direction found for room: " + startRoom.roomData.RoomName);
        }
    }
    private void GenerateEntryForRoom(RoomPlacement startRoom)
    {
        // Get a random enabled direction as Vector2 offset
        Vector2? directionOffset = startRoom.roomData.GetRandomEnabledDirection();
        if (!directionOffset.HasValue)
        {
            Debug.LogWarning("No enabled direction found for room: " + startRoom.roomData.RoomName);
            return;
        }

        Vector2 position = Vector2.zero;
        if(startRoom.roomData.MergeAmount > 4)
        {
            Debug.Log($"MergeEverything{startRoom.roomData.name}{directionOffset.HasValue}");

            position = startRoom.Position + (directionOffset.Value * ((startRoom.roomData.Size.x / 2) +5));
            if (ShowDebug)
            {
                DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.blue);
            }
            bossEntry = new Vector3(position.x, 0, position.y);
            startRoom.roomData.SetDirection(directionOffset.Value, false);
            return;
        }
        
        position = startRoom.Position + (directionOffset.Value * (startRoom.roomData.Size.x / 2));
        if (ShowDebug)
        {
            DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.blue);
        }
        entrys.Add(new Vector2Int((int)position.x, (int)position.y));
        startRoom.roomData.SetDirection(directionOffset.Value, false);
    }

    public int gridSize = 5; // Distance between points
    public Vector2Int gridBounds = new Vector2Int(100, 100); // Grid size (width x height)
    private Dictionary<Vector2Int, bool> pointCloud = new Dictionary<Vector2Int, bool>();
    private List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();
}
