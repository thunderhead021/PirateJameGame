using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public List<SO_Room> RoomList;
    public List<SO_LevelInstance> LevelInstances;
    public int CurrentLevelIndex = 0;
    public int Seed;
    public int MinIntermediaryRooms = 1;
    public int MaxIntermediaryRooms = 3;
    public int MinDistanceBetweenRooms = 2;
    public int MaxDistanceBetweenRooms = 5;
    public bool ShowDebug = true;
    public bool generateMap = false;
    public void Update()
    {
        if(generateMap == true)
        {
            LoadLevel(CurrentLevelIndex);

            Debug.Log("GeneratingMap");
            generateMap = false;
        }
    }
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

    public void GenerateMap()
    {
        ClearMap();
        Random.InitState(Seed);

        Vector2Int currentPosition = Vector2Int.zero;
        occupiedPositions.Clear();

        foreach (SO_Room room in RoomList)
        {
            currentPosition = PlaceRoom(room, currentPosition);
            int intermediaryRoomCount = Random.Range(MinIntermediaryRooms, MaxIntermediaryRooms + 1);
            currentPosition = GeneratePath(currentPosition, intermediaryRoomCount);
        }
    }

    private Vector2Int PlaceRoom(SO_Room roomData, Vector2Int currentPosition)
    {
        Vector2Int position;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            position = currentPosition + new Vector2Int(
                Random.Range(MinDistanceBetweenRooms, MaxDistanceBetweenRooms + 1),
                Random.Range(MinDistanceBetweenRooms, MaxDistanceBetweenRooms + 1)
            );
            attempts++;
            if (attempts > maxAttempts)
            {
                Debug.LogWarning($"Failed to place room {roomData.RoomName} after {maxAttempts} attempts. Skipping...");
                return currentPosition;
            }
        } while (CheckOverlap(position, roomData.Size));

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
            if (position.x < placement.Position.x + placement.Size.x + MinDistanceBetweenRooms &&
                position.x + size.x + MinDistanceBetweenRooms > placement.Position.x &&
                position.y < placement.Position.y + placement.Size.y + MinDistanceBetweenRooms &&
                position.y + size.y + MinDistanceBetweenRooms > placement.Position.y)
            {
                return true;
            }
        }
        return false;
    }

    private Vector2Int GeneratePath(Vector2Int start, int intermediaryRoomCount)
    {
        Vector2Int current = start;
        for (int i = 0; i < intermediaryRoomCount; i++)
        {
            Vector2Int next = current + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
            if (ShowDebug)
            {
                DebugRenderer.DrawPath(current, next);
            }
            current = next;
        }
        return current;
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
        GenerateMap();
    }

    public void NextLevel()
    {
        LoadLevel((CurrentLevelIndex + 1) % LevelInstances.Count);
    }

    public void PreviousLevel()
    {
        LoadLevel((CurrentLevelIndex - 1 + LevelInstances.Count) % LevelInstances.Count);
    }

    private void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        DebugRenderer.ClearDebug();
    }
}
