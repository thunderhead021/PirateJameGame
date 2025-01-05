using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum OpeningDirection
{
    none,
    bottom,
    top,
    left,
    right,
}

public enum Order
{
    first,
    second,
    third,
}

public struct OpenOrder
{
    public OpeningDirection opening { get; }
    public Order order { get; }

    public OpenOrder(OpeningDirection openingDirection, Order order)
    {
        opening = openingDirection;
        this.order = order;
    }

    // Override Equals to ensure correct comparisons
    public override bool Equals(object obj)
    {
        if (obj is OpenOrder other)
        {
            return opening == other.opening && order == other.order;
        }
        return false;
    }

    // Override GetHashCode for consistent hashing
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(opening, order);
    }

    // ToString for debugging
    public override readonly string ToString()
    {
        return $"({opening}, {order})";
    }
}


public class Cell
{
    public bool visited = false;
    public HashSet<OpenOrder> status = new();
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
                        Order order = getRandomOrder();
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.left, order));
                        curCell = newCell;
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.right, order));
                    }
                    else
                    {
                        Order order = getRandomOrder();
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.top, order));
                        curCell = newCell;
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.bottom, order));
                    }
                }
                else 
                {
                    if (newCell + 1 == curCell)
                    {
                        Order order = getRandomOrder();
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.right, order));
                        curCell = newCell;
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.left, order));
                    }
                    else
                    {
                        Order order = getRandomOrder();
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.bottom, order));
                        curCell = newCell;
                        board[curCell].status.Add(new OpenOrder(OpeningDirection.top, order));
                    }
                }
            }
        }

        MapManager.instance.GenerateLevel();
    }

    private Order getRandomOrder() 
    {
        Array value = Enum.GetValues(typeof(Order));
        return (Order)value.GetValue(Random.Range(0, value.Length));
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
