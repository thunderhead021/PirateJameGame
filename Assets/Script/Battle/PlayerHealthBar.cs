using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    private void Awake()
    {
        Setup(PlayerManager.instance.playerData.HP);
        parent = PlayerManager.instance.gameObject;
    }

    public override void Death()
    {
        Debug.Log("Game over");
    }

    public override void ModifyHealth(float amount)
    {
        base.ModifyHealth(amount);
        if (maxHealth > 0) 
        {
            PlayerManager.instance.playerData.HP = maxHealth;
        }
    }
}
