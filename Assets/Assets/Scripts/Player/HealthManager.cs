using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Animator animator;

    public Image healthBar;
    public int currentHealth;
    public int maxHealth = 100;

    [Header("Damage Settings")]
    public int damageOnHit = 10;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        // Camera shake
        if (CameraShake.Instance != null)
            StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.1f));

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        Debug.Log("Player took damage! Current HP: " + currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void Die()
    {   
        Debug.Log("Player has died.");
        animator.SetTrigger("Die");
        PlayerController player = GetComponent<PlayerController>();
        if (player != null)
            player.isDead = true;
        Destroy(gameObject, 1.7f);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
