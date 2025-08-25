using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBase : MonoBehaviour
{
    [Header("Health UI")]
    public Slider healthSlider;   // Kéo Slider từ Canvas vào đây
    public Text healthText;       // Kéo Text từ Canvas vào đây

    public int maxHealth = 100;
    public int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null) healthSlider.maxValue = maxHealth;
        UpdateUI();
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateUI();
        if (currentHealth <= 0) Die();
    }

    public virtual void Heal(int healAmount)
    {
        if (healAmount <= 0) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (healthText != null) healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public int GetCurrentHealth() => currentHealth;

    protected abstract void Die();
}
