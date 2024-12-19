using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    public int RoomRandomnessMax = 5;
    public bool ShowDebug = true;
    public bool generateMap = false;
    private List<Vector2Int> exits;
    private List<Vector2Int> entrys;
    private Vector2Int bossEntry;
    [SerializeField]private float pathRandom = 0.5f;
    [SerializeField]private int maxPathLength = 80;

    Vector2Int[] offsets = new Vector2Int[]
    {
        new Vector2Int(-5, 0),  // Left
        new Vector2Int(5, 0),   // Right
        new Vector2Int(0, -5),  // Down
        new Vector2Int(0, 5),   // Up
        new Vector2Int(-5, -5), // Bottom-left
        new Vector2Int(5, -5),  // Bottom-right
        new Vector2Int(-5, 5),  // Top-left
        new Vector2Int(5, 5)    // Top-right
    };

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
    public int gridSize = 10; // Distance between points
    public Vector2Int gridBounds = new Vector2Int(200,400 ); // Grid size (width x height)
    private Dictionary<Vector2Int, bool> pointCloud = new Dictionary<Vector2Int, bool>();
    private List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();

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
        bossEntry = new Vector2Int(0,0);

        Vector2Int spawnPosition = Vector2Int.zero;
        occupiedPositions.Clear();
        
        InitializePointCloud();

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
        roomObject.transform.parent = this.gameObject.transform;
        roomObject.name = roomData.RoomName;
        roomObject.transform.position = new Vector3(position.x, 0, position.y);
        RoomInstance instance = roomObject.AddComponent<RoomInstance>();
        instance.SetupRoom(roomData, position);
        occupiedPositions.Add(new RoomPlacement(position, roomData.Size, roomData));

        // Check if the point exists in the point cloud
        if (pointCloud.ContainsKey(position))
        {
            pointCloud[position] = false; // Mark it as unavailable
        }
        foreach (var offset in offsets)
        {
            Vector2Int surroundingPoint = position + offset;

            if (pointCloud.ContainsKey(surroundingPoint))
            {
                pointCloud[surroundingPoint] = false; // Mark surrounding point as unavailable
            }
        }
        
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
        Debug.Log(t);

        Vector2Int position = new Vector2Int(Random.Range(-(RoomRandomnessMax + 10), RoomRandomnessMax + 10), positionY + Random.Range(-RoomRandomnessMax, RoomRandomnessMax));
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

        gridBounds.y = LevelInstances[levelIndex].LevelDistance;
        // Clone each room from the level's RoomList and add it to RoomList
        spawnRoom = spawnRoom.Clone();
        bossRoom = bossRoom.Clone();
        RoomList = new List<SO_Room>();
        foreach (var room in level.SpecialRoomList)
        {
            RoomList.Add(room.Clone());
        }

        Seed = level.Seed;
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
                
                GeneratePath(exit, closestEntry.Value, pathRandom, maxPathLength);
                entrys.Remove(closestEntry.Value);
            }
            else
            {
                GeneratePath(exit, bossEntry, pathRandom, maxPathLength);
            }
            
        }
        foreach (var path in completedPaths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                InstRoom(hallwayRoom, path[i]);
            }
        }
    }
    private void GenerateExitForRoom(RoomPlacement startRoom)
    {
        Vector2? directionOffset = startRoom.roomData.GetRandomEnabledDirection();

        if (directionOffset.HasValue)
        {
            Vector2 position = startRoom.Position + (directionOffset.Value * ((startRoom.roomData.Size.x / 2)+ 5));
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

            position = startRoom.Position + (directionOffset.Value * ((startRoom.roomData.Size.x / 2) + 5));
            if (ShowDebug)
            {
                DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.blue);
            }
            bossEntry = new Vector2Int((int)position.x, (int)position.y);
            startRoom.roomData.SetDirection(directionOffset.Value, false);
            return;
        }
        
        position = startRoom.Position + (directionOffset.Value * ((startRoom.roomData.Size.x / 2) + 5));
        if (ShowDebug)
        {
            DebugRenderer.DrawDebugSphere(new Vector3(position.x, 0, position.y), 0.5f, Color.blue);
        }
        Vector2Int positionInt = new Vector2Int((int)position.x, (int)position.y);
        entrys.Add(positionInt);
        startRoom.roomData.SetDirection(directionOffset.Value, false);
    }
    void InitializePointCloud()
    {
        for (int y = 0; y <= gridBounds.y; y += gridSize) // Y-direction loop
        {
            for (int x = -gridBounds.x / 2; x <= gridBounds.x / 2; x += gridSize) // X-direction loop
            {
                Vector2Int point = new Vector2Int(x, y); // Replace Z with Y
                pointCloud[point] = true; // All points start as available
            }
        }
    }
    void GeneratePath(Vector2Int exit, Vector2Int entry, float randomnessValue, int maxPathLength)
    {
        if (!pointCloud.ContainsKey(exit) || !pointCloud.ContainsKey(entry))
        {
            Debug.LogError($"Exit or Entry point is outside the grid bounds: {exit}, {entry}");
            return;
        }

        List<Vector2Int> path = new List<Vector2Int>();
        Stack<Vector2Int> backtrackStack = new Stack<Vector2Int>(); // For backtracking
        HashSet<Vector2Int> visitedPoints = new HashSet<Vector2Int>(); // Temporary marking
        int retryCount = 0;
        const int maxRetries = 100;

        randomnessValue = Mathf.Clamp01(randomnessValue); // Ensure randomness is between 0 and 1
        const float maxRandomnessScale = 0.8f; // Maximum scaling factor for randomness

        Vector2Int currentPoint = exit;
        path.Add(currentPoint);
        visitedPoints.Add(currentPoint);

        while (currentPoint != entry)
        {
            List<Vector2Int> neighbors = GetAvailableNeighbors(currentPoint).Where(p => !visitedPoints.Contains(p) && !IsOtherExitOrEntry(p, exit, entry)).ToList();

            if (neighbors.Count > 0)
            {
                // Calculate the percentage of the path length relative to maxPathLength
                float pathPercentage = Mathf.Clamp01((float)path.Count / maxPathLength);

                // Scale randomness based on the path percentage
                float scaledRandomness = randomnessValue * (1 - pathPercentage) * maxRandomnessScale;

                // Choose a neighbor based on scaled randomness
                Vector2Int selectedNeighbor;
                if (Random.value < scaledRandomness)
                {
                    // Choose a random neighbor
                    selectedNeighbor = neighbors.OrderBy(_ => UnityEngine.Random.value).First();
                }
                else
                {
                    // Choose the neighbor closest to the entry
                    float closestDist = neighbors.Min(p => Vector2Int.Distance(p, entry));
                    selectedNeighbor = neighbors.Where(p => Mathf.Approximately(Vector2Int.Distance(p, entry), closestDist)).OrderBy(_ => UnityEngine.Random.value).First();
                }

                currentPoint = selectedNeighbor;
                path.Add(currentPoint);
                visitedPoints.Add(currentPoint);
                backtrackStack.Push(currentPoint); // Push to stack for backtracking
                retryCount = 0; // Reset retry count
            }
            else if (backtrackStack.Count > 0)
            {
                // Backtrack to the last point
                currentPoint = backtrackStack.Pop();
                path.RemoveAt(path.Count - 1); // Remove the last point from the path
                retryCount++;
            }
            else
            {
                Debug.LogWarning("No more neighbors and backtracking exhausted. Path generation failed.");
                return;
            }

            if (retryCount >= maxRetries)
            {
                Debug.LogWarning("Exceeded retry limit. Path generation stopped.");
                return;
            }
        }

        path.Add(entry);
        completedPaths.Add(path);
        MarkPathAsUsed(path);
    }

    /// <summary>
    /// Checks if the point is another exit or entry, excluding the current ones.
    /// </summary>
    bool IsOtherExitOrEntry(Vector2Int point, Vector2Int currentExit, Vector2Int currentEntry)
    {
        // Assuming you have a list or set of all exits and entries
        return (exits.Contains(point) || entrys.Contains(point)) && point != currentExit && point != currentEntry;
    }
    List<Vector2Int> GetAvailableNeighbors(Vector2Int point)
    {
        int tolGridSize = gridSize;

        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            point + new Vector2Int(tolGridSize, 0),
            point + new Vector2Int(-tolGridSize, 0),
            point + new Vector2Int(0, tolGridSize),
            point + new Vector2Int(0, -tolGridSize),
        };

        return neighbors.Where(p => pointCloud.ContainsKey(p) && pointCloud[p]).ToList();
    }
    void MarkPathAsUsed(List<Vector2Int> path)
    {
        foreach (var point in path)
        {
            pointCloud[point] = false;
        }
    }
    void OnDrawGizmos()
    {
        if (pointCloud == null || pointCloud.Count == 0) return;

        Gizmos.color = Color.gray;
        foreach (var point in pointCloud)
        {
            if (point.Value) // Available points
                Gizmos.DrawSphere(new Vector3(point.Key.x, 0, point.Key.y), 0.5f);
        }

        Gizmos.color = Color.green;
        foreach (var path in completedPaths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector3(path[i].x, 0, path[i].y), new Vector3(path[i + 1].x, 0, path[i + 1].y));
            }
        }
    }
}