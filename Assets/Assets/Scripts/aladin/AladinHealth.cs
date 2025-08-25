using Enemies.Aladin;
using UnityEngine;

public class AladinHealth : EnemyHealthSystem
{
    private Animator animator;
    private bool isDead = false;

    protected override void Start()
    {
        maxHealth = 15;
        base.Start();
    }
    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");

        if (animator != null)
        {
            animator.SetTrigger("Explode (Read-Only)");
        }

        // Tắt collider để không bị đánh tiếp
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        AladinChase chase = GetComponent<AladinChase>();
        if (chase != null) chase.enabled = false;

        Destroy(gameObject, 1.8f);
    }
}
