using UnityEngine;
using System.Collections.Generic;

public class MapGenManager : MonoBehaviour
{
    public List<SO_Room> RoomList;
    public List<SO_LevelInstance> LevelInstances;
    public int CurrentLevelIndex = 0;
    public int Seed;
    public int MinIntermediaryRooms = 1;
    public int MaxIntermediaryRooms = 3;
    public bool ShowDebug = true;
    public bool generateMap = false;
    public int MinDistanceBetweenRooms = 2;
    public int MaxDistanceBetweenRooms = 5;
    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();

    public void Update()
    {
        if(generateMap == true)
        {
            LoadLevel(CurrentLevelIndex);
            GenerateMap();
            Debug.Log("GeneratingMap");
            generateMap = false;
        }
    }

    public void GenerateMap()
    {
        ClearMap();
        Random.InitState(Seed);

        Vector2Int currentPosition = Vector2Int.zero;
        foreach (SO_Room room in RoomList)
        {
            PlaceRoom(room, currentPosition);
            int intermediaryRoomCount = Random.Range(MinIntermediaryRooms, MaxIntermediaryRooms + 1);
            currentPosition = GeneratePath(currentPosition, intermediaryRoomCount);
        }
    }

    
    private Vector2Int PlaceRoom(SO_Room roomData, Vector2Int currentPosition)
    {
        Vector2Int position;
        do
        {
            position = currentPosition + new Vector2Int(
                Random.Range(MinDistanceBetweenRooms, MaxDistanceBetweenRooms + 1),
                Random.Range(MinDistanceBetweenRooms, MaxDistanceBetweenRooms + 1)
            );
        } while (CheckOverlap(position, roomData.Size));

        occupiedPositions.Add(position);

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
        foreach (Vector2Int occupied in occupiedPositions)
        {
            if (position.x < occupied.x + size.x && position.x + size.x > occupied.x &&
                position.y < occupied.y + size.y && position.y + size.y > occupied.y)
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