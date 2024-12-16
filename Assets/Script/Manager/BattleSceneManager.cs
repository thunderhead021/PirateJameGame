using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public Targeting targeting;
    public GameObject cancleBtn;
    public GameObject ItemsList;
    public GameObject selectionsList;
    public HealthBar playerHealthBar;

    [HideInInspector]
    public bool canTarget = false;

    public static BattleSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Cancle() 
    {
        cancleBtn.SetActive(false);
        selectionsList.SetActive(true);
        targeting.ResetTarget();
        targeting.targetType = TargetType.None;
        canTarget = false;
    }

    public void Selected() 
    {
        cancleBtn.SetActive(true);
        ItemsList.SetActive(false);
    }
}
