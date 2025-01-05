using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    //up down left right
    public Wall[] walls;


    //public List<OpeningDirection> openingDirections;
    //public List<Order> orders;

    //private void Start()
    //{
    //    HashSet<OpenOrder> openOrders = new();
    //    for(int i = 0; i < openingDirections.Count; i++) 
    //    {
    //            openOrders.Add(new OpenOrder(openingDirections[i], orders[i]));
    //    }
    //    UpdateRoom(openOrders);
    //}

    public void UpdateRoom(HashSet<OpenOrder> openingDirections) 
    {
        foreach (OpenOrder openingDirection in openingDirections) 
        {
            switch (openingDirection.opening)
            {
                case OpeningDirection.top:
                    walls[0].SetDoor(openingDirection.order);
                    break;
                case OpeningDirection.bottom:
                    walls[1].SetDoor(openingDirection.order);
                    break;
                case OpeningDirection.left:
                    walls[2].SetDoor(openingDirection.order);
                    break;
                case OpeningDirection.right:
                    walls[3].SetDoor(openingDirection.order);
                    break;
            }
        }
    }
}
