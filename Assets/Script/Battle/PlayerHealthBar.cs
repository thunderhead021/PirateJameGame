using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    public override void Death()
    {
        Debug.Log("Game over");
    }
}
