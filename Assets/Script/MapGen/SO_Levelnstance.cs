using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Map/Level Instance")]
public class SO_LevelInstance : ScriptableObject
{
    public string LevelName;
    public List<SO_Room> SpecialRoomList;
    public List<SO_Room> NormalRoomPool;
    public int Seed;
    public int LevelDistance;
    public int RoomRandomnessMax;
    public int RoomRandomnessMin;
}