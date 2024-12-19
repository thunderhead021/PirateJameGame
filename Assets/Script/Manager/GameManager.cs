using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Encounter curEncounter;
    public FloatingText floatingText;

    private void Awake()
    {
        instance = this;
    }

    public void SetCurEncounter(Encounter encounter) 
    {
        curEncounter.enemies.Clear();
        curEncounter.enemies = encounter.enemies;
    }

    public void ShowFloatingText(GameObject parent, string text, bool isEnemy) 
    {
        GameObject newFloatingText = Instantiate(floatingText.gameObject, parent.transform, false);
        newFloatingText.SetActive(false);
        newFloatingText.GetComponent<FloatingText>().SetText(text, isEnemy);
    }
}
