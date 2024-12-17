using UnityEngine;
using System.Collections.Generic;

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

        public RoomPlacement(Vector2Int position, Vector2Int size)
        {
            Position = position;
            Size = size;
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
        foreach(var room in RoomList)
        {
            PlaceRoom(room, spawnPosition);
        }

        // Generate paths between rooms
        //GeneratePaths();
    }

    private void PlaceRoomExact(SO_Room roomData, Vector2Int position)
    {
        occupiedPositions.Add(new RoomPlacement(position, roomData.Size));

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
        int roomIndex = occupiedPositions.Count -1;
        float t = (float)roomIndex / (RoomList.Count + 1);
        int positionY = Mathf.RoundToInt(totalDistance * t);
        Debug.Log($"Placing room{roomIndex},Percentage{t},Position{positionY}");

        // Apply randomness within the defined range
        Vector2Int position = new Vector2Int(
            Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1),
            positionY + Random.Range(-RoomRandomnessMax, RoomRandomnessMax + 1)
        );

        // Check for overlaps
        if (CheckOverlap(position, roomData.Size))
        {
            return spawnPosition; // Return spawn position if overlap detected
        }

        // Place room
        occupiedPositions.Add(new RoomPlacement(position, roomData.Size));

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
        RoomList = new List<SO_Room>(level.RoomList);
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
}
