using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Encounter curEncounter;

    private void Awake()
    {
        instance = this;
    }

    public void SetCurEncounter(Encounter encounter) 
    {
        curEncounter.enemies.Clear();
        curEncounter.enemies = encounter.enemies;
    }
}
