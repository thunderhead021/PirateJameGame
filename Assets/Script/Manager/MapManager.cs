using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Room[] normalRooms;
    public Room[] bossRooms;
    public Room[] miniBoosRooms;
    public Room[] enemyRooms;
    public Room[] restRooms;
    public Room[] startRooms;

    public Vector2 roomSize;
    public float roomYPos;

    public static MapManager instance;
    public void Awake()
    {
        instance = this;
    }

    public Room GetARoom(bool isBoss = false, bool start = false) 
    {
        if (isBoss)
            return bossRooms[Random.Range(0, bossRooms.Length)];

        if (start)
            return startRooms[Random.Range(0, startRooms.Length)];

        return LevelManager.instance.GetARoomType() switch
        {
            RoomType.Rest => restRooms[Random.Range(0, restRooms.Length)],
            RoomType.Enemy => enemyRooms[Random.Range(0, enemyRooms.Length)],
            RoomType.Mini => miniBoosRooms[Random.Range(0, miniBoosRooms.Length)],
            RoomType.Boss => bossRooms[Random.Range(0, bossRooms.Length)],
            _ => normalRooms[Random.Range(0, normalRooms.Length)],
        };
    }

    public void GenerateLevel() 
    {
        for (int x = 0; x < MapGenerator.instance.size.x; x++) 
        {
            for (int y = 0; y < MapGenerator.instance.size.y; y++) 
            {
                Cell curCell = MapGenerator.instance.board[Mathf.FloorToInt(x + y*MapGenerator.instance.size.x)];
                if (curCell.visited) 
                {
                    CreateRoom(new Vector3(x * roomSize.x, roomYPos, -y * roomSize.y), curCell.status, x + "-" + y, Mathf.FloorToInt(x + y * MapGenerator.instance.size.x) == MapGenerator.instance.board.Count - 1, Mathf.FloorToInt(x + y * MapGenerator.instance.size.x) == 0);
                }
            }
        }
    }

    private void CreateRoom(Vector3 postion, HashSet<OpenOrder> openingDirections, string name, bool isBoss, bool isStart)
    {
        Room room = GetARoom(isBoss, isStart);
        GameObject newRoom = Instantiate(room.gameObject, postion, Quaternion.identity, transform);
        newRoom.GetComponent<Room>().UpdateRoom(openingDirections);
        newRoom.name += name;
    }
}
