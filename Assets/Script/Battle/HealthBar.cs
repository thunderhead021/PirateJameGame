using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100f;
    public Slider healthBar;
    public GameObject parent;

    public void ModifyHealth(float amount) 
    {
        maxHealth -= amount;
        maxHealth = Mathf.Max(maxHealth, 0); 
        maxHealth = Mathf.Min(maxHealth, healthBar.maxValue);
        healthBar.value = maxHealth;
        if (maxHealth == 0)
        {
            Death();
        }
    }

    public void Setup(float maxHp) 
    {
        maxHealth = maxHp;
        healthBar.maxValue = maxHp;
        healthBar.value = maxHp;
    }

    public virtual void Death() 
    {
        Destroy(parent);
    }
}
