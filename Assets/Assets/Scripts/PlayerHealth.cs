using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        base.UpdateUI();
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        base.UpdateUI();
    }

    protected override void Die()
    {
        Debug.Log("Player died!");

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        Destroy(gameObject, 1.5f);
    }
}
