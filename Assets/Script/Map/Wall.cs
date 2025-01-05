using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject[] notDoors;

    public void SetDoor(Order order) 
    {
        foreach (GameObject go in doors) 
            go.SetActive(false);

        foreach (GameObject go in notDoors)
            go.SetActive(true);

        switch (order) 
        {
            case Order.first:
                doors[0].SetActive(true);
                notDoors[0].SetActive(false);
                break;
            case Order.second:
                doors[1].SetActive(true);
                notDoors[1].SetActive(false);
                break;
            case Order.third:
                doors[2].SetActive(true);
                notDoors[2].SetActive(false);
                break;
        }
    }
}
