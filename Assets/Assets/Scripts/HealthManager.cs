using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public int currentHealth;
    public int maxHealth = 100;


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            takeDamage(10);
        }
        if (currentHealth <= 0)
        {
            die();
        }
    }

    void takeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }
}