using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Map/Room")]
public class SO_Room : ScriptableObject
{
    public string RoomName;
    public int SplitAmount;
    public int MergeAmount;
    public Vector2Int Size;
    public GameObject RoomPrefab;
    public int MinDistanceFromStart;
    public int MaxDistanceFromStart;
}