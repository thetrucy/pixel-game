using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public int currentHealth;
    public int maxHealth = 100;

    [Header("Damage Settings")]
    public int damageOnHit = 10; // Số máu mất khi va vào enemy

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu chạm vào enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageOnHit);
        }
    }

    // Nếu dùng trigger thì đổi sang cái này:
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(damageOnHit);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log("Player took damage! Current HP: " + currentHealth);
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
