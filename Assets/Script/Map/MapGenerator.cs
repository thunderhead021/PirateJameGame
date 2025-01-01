using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpeningDirection
{
    none,
    bottom,
    top,
    left,
    right,
}

public class Cell
{
    public bool visited = false;
    public HashSet<OpeningDirection> status = new();
}

public class MapGenerator : MonoBehaviour
{
    public Vector2 size = new();
    public int startPos = 0;

    [HideInInspector]
    public List<Cell> board = new();

    public static MapGenerator instance;
    public void Awake()
    {
        instance = this;
    }

    public void CreateMap() 
    {
        for (int x = 0; x < size.x; x++) 
        {
            for (int y = 0; y < size.y; y++) 
            {
                board.Add(new Cell());
            }
        }

        int curCell = startPos;

        Stack<int> path = new();

        int k = 0;

        while (k < 1000) 
        {
            k++;

            board[curCell].visited = true;

            if (curCell >= board.Count - 1 && LevelManager.instance.numberOfRestRooms + LevelManager.instance.numberOfEnemyRooms + LevelManager.instance.numberOfMiniBossRooms + LevelManager.instance.numberOfNormalRooms == board.Count - 1 ) 
                break;

            List<int> neighbors = CheckNeighbors(curCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    curCell = path.Pop();
                }
            }
            else 
            {
                path.Push(curCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > curCell)
                {
                    if (newCell - 1 == curCell)
                    {
                        board[curCell].status.Add(OpeningDirection.left);
                        curCell = newCell;
                        board[curCell].status.Add(OpeningDirection.right);
                    }
                    else
                    {
                        board[curCell].status.Add(OpeningDirection.top);
                        curCell = newCell;
                        board[curCell].status.Add(OpeningDirection.bottom);
                    }
                }
                else 
                {
                    if (newCell + 1 == curCell)
                    {
                        board[curCell].status.Add(OpeningDirection.right);
                        curCell = newCell;
                        board[curCell].status.Add(OpeningDirection.left);
                    }
                    else
                    {
                        board[curCell].status.Add(OpeningDirection.bottom);
                        curCell = newCell;
                        board[curCell].status.Add(OpeningDirection.top);
                    }
                }
            }
        }

        MapManager.instance.GenerateLevel();
    }

    private List<int> CheckNeighbors(int cell) 
    {
        List<int> result = new();

        //up
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited) 
        {
            result.Add(Mathf.FloorToInt(cell - size.x));
        }

        //down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            result.Add(Mathf.FloorToInt(cell + size.x));
        }

        //right
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            result.Add(Mathf.FloorToInt(cell + 1));
        }

        //left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            result.Add(Mathf.FloorToInt(cell - 1));
        }

        return result;
    }
}
