using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Encounter curEncounter;
    public FloatingText floatingText;
    public GameObject playerChar;

    [HideInInspector]
    public SceneID curScene = SceneID.MainGameScene;

    [SerializeField]
    private Vector3 playerPos;
   
    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (curScene == SceneID.MainGameScene)
        {
            if (playerChar != null)
            {
                playerPos = playerChar.transform.position;
            }
            else 
            {
                playerChar = GameObject.FindGameObjectWithTag("PlayerUnit");
                playerChar.transform.position = playerPos;
            }
        }
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
