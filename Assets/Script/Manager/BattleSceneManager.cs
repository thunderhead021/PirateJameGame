using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public Targeting targeting;
    public GameObject cancleBtn;
    public GameObject ItemsList;
    public GameObject selectionsList;
    public GameObject enemyTurn;
    public HealthBar playerHealthBar;
    public GameObject TurnButton;
    public Turns turnUI;

    [HideInInspector]
    public bool canTarget = false;

    public static BattleSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (GameManager.instance.curEncounter != null) 
        {
            List<TurnHelper> order = new()
            {
                new TurnHelper()
                {
                    baseEntity = PlayerManager.instance.playerData,
                    gameObject = PlayerManager.instance.gameObject,
                }
            };
            for (int i = 0; i < targeting.enemies.Count; i++) 
            {
                if (i <= GameManager.instance.curEncounter.enemies.Count - 1)
                {
                    var model = targeting.enemies[i].GetComponent<CreateModel>().CreateEnemyModel(GameManager.instance.curEncounter.enemies[i], GameManager.instance.curEncounter.enemies[i].entityData.HP);
                    TurnHelper enemy = new() 
                    {
                        baseEntity = GameManager.instance.curEncounter.enemies[i].entityData,
                        gameObject = model,
                    };
                    order.Add(enemy);
                }
                else 
                {
                    targeting.enemies[i].SetActive(false);
                }
            }
            GameManager.instance.curEncounter = null;
            order.Sort((a,b) => a.baseEntity.Speed.CompareTo(b.baseEntity.Speed));
            turnUI.Setup(order);
        }  
    }

    public void Cancle() 
    {
        cancleBtn.SetActive(false);
        selectionsList.SetActive(true);
        targeting.ResetTarget();
        targeting.curObject = null;
        canTarget = false;
    }

    public void Selected() 
    {
        cancleBtn.SetActive(true);
        ItemsList.SetActive(false);
    }

    public void SetEnemyTurn(bool isEnemy) 
    {
        enemyTurn.SetActive(isEnemy);
        selectionsList.SetActive(!isEnemy);
        targeting.ResetTarget();
        canTarget = false;
        targeting.curObject = null;
    }
}

[System.Serializable]
public class TurnHelper 
{
    public BaseEntity baseEntity;
    public GameObject gameObject;
}