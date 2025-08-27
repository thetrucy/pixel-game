using Enemies.Alibaba;
using UnityEngine;

public class AlibabaHealth : EnemyHealthSystem
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool isDead = false;

    protected override void Start()
    {
        maxHealth = 20;
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        // Báo cho manager bớt 1 quái
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy();
        }

        GetComponent<Animator>().SetTrigger("Die");

        if (rb != null) rb.linearVelocity = Vector2.zero;

        AlibabaChase chase = GetComponent<AlibabaChase>();
        if (chase != null) chase.enabled = false;

        base.Die();
        Destroy(gameObject, 1.01f);
    }
}
