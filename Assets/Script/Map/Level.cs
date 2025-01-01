using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public int numberOfNormalRooms = 10;
    public int numberOfRestRooms;
    public int numberOfMiniBossRooms;
    public int numberOfEnemyRooms;

}
