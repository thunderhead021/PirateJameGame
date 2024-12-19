using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAttackName", menuName = "Attacks/New Damage Attack")]
public class BaseAttack : BaseObject
{
    public float damage = 1f;

    public override void DoThing(GameObject target)
    {
        if (target.CompareTag("Enemy") && target.GetComponent<CreateModel>() != null)
        {
            target.GetComponent<CreateModel>().healthBar.ModifyHealth(damage);
        }
        else if (target.CompareTag("Player")) 
        {
            BattleSceneManager.instance.playerHealthBar.ModifyHealth(damage);
        }
    }
}
