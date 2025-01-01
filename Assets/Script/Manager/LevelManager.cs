using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum RoomType 
{
    Hall,
    Rest,
    Mini,
    Enemy,
    Boss
}

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    public Vector3 playerStartingPos;

    private int curLevel = 0;

    [HideInInspector]
    public int numberOfNormalRooms = 10;
    [HideInInspector]
    public int numberOfRestRooms = 0;
    [HideInInspector]
    public int numberOfMiniBossRooms = 0;
    [HideInInspector]
    public int numberOfEnemyRooms = 0  ;

    public bool generated = false;  

    public static LevelManager instance;
    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (levels.Length > 0)
        {
            Level level = levels[curLevel];
            if (level != null)
            {
                numberOfNormalRooms = level.numberOfNormalRooms;
                numberOfRestRooms = level.numberOfRestRooms;
                numberOfMiniBossRooms = level.numberOfMiniBossRooms;
                numberOfEnemyRooms = level.numberOfEnemyRooms;
                CreateLevel();
            }
        }
    }

    private void CreateLevel() 
    {
        if (!generated) 
        {
            MapGenerator.instance.size = GetVectorFromInt(numberOfNormalRooms + numberOfRestRooms + 1 + numberOfMiniBossRooms + numberOfEnemyRooms);
            MapGenerator.instance.CreateMap();
            GameManager.instance.playerChar.transform.position = playerStartingPos;
            generated = true;
        }
    }

    private Vector2 GetVectorFromInt(int number)
    {
        for (int x = 2; x <= Mathf.Abs(number); x++)
        {
            if (number % x == 0)
            {
                int y = number / x; 
                return new Vector2(x, y);
            }
        }

        return GetVectorFromInt(number + 1);
    }


    public RoomType GetARoomType() 
    {
        List<RoomType> roomTypes = new();

        if (numberOfNormalRooms > 0)
            roomTypes.Add(RoomType.Hall);

        if (numberOfEnemyRooms > 0)
            roomTypes.Add(RoomType.Enemy);

        if (numberOfRestRooms > 0)
            roomTypes.Add(RoomType.Rest);

        if (numberOfMiniBossRooms > 0)
            roomTypes.Add(RoomType.Mini);

        int randomIndex = Random.Range(0, roomTypes.Count);

        switch (roomTypes[randomIndex]) 
        {
            case RoomType.Mini:
                numberOfMiniBossRooms--;
                break;
            case RoomType.Rest:
                numberOfRestRooms--;
                break;
            case RoomType.Enemy:
                numberOfEnemyRooms--;
                break;
            case RoomType.Hall:
                numberOfNormalRooms--;
                break;
        }

        return roomTypes[randomIndex];
    }
}
