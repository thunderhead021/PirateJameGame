using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    //up down left right
    public GameObject[] walls;
    //public List<OpeningDirection> openingDirections;

    //private void Start()
    //{
    //    UpdateRoom(openingDirections.ToHashSet());
    //}

    public void UpdateRoom(HashSet<OpeningDirection> openingDirections) 
    {
        foreach (GameObject wall in walls) 
        {
            wall.SetActive(true);
        }

        foreach (OpeningDirection openingDirection in openingDirections) 
        {
            switch (openingDirection)
            {
                case OpeningDirection.top:
                    walls[0].SetActive(false);
                    break;
                case OpeningDirection.bottom:
                    walls[1].SetActive(false);
                    break;
                case OpeningDirection.left:
                    walls[2].SetActive(false);
                    break;
                case OpeningDirection.right:
                    walls[3].SetActive(false);
                    break;
            }
        }
    }
}
