using UnityEngine;

public class CreateModel : MonoBehaviour
{
    public HealthBar healthBar;
    public BaseEnemy entity;

    public GameObject CreateEnemyModel(BaseEnemy baseEnemy, float maxHp) 
    {
        entity = baseEnemy;
        GameObject enemyModel = Instantiate(baseEnemy.Model);
        enemyModel.transform.SetParent(transform, false);
        healthBar.Setup(maxHp);
        return gameObject;
    }
}
