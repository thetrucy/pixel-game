using Enemies.Aladin;
using UnityEngine;

public class AladinHealth : EnemyHealthSystem
{
    private Animator animator;
    private bool isDead = false;
    private bool hasExploded = false;
    private float lastDamageTime = -Mathf.Infinity;

    [Header("Explosion Settings")]
    public Collider2D explosionCollider; // collider riêng cho vụ nổ
    public int explosionDamage = 20;

    protected override void Start()
    {
        maxHealth = 15;
        animator = GetComponent<Animator>();

        if (explosionCollider != null)
            explosionCollider.enabled = false;

        base.Start();
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        AladinChase chase = GetComponent<AladinChase>();
        if (chase != null) chase.enabled = false;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // xoay về đúng chiều animation nổ

        // Sau 1s thì phát nổ
        Invoke(nameof(Explode), 1f);

        base.Die();

        Destroy(gameObject, 1.3f);
    }

    private void Explode()
    {
        if (explosionCollider == null) return;

        hasExploded = false; // reset flag trước khi bật
        explosionCollider.enabled = true;
        explosionCollider.isTrigger = true;

        Invoke(nameof(DisableExplosion), 0.2f);
    }

    private void DisableExplosion()
    {
        if (explosionCollider != null)
            explosionCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vụ nổ chỉ gây damage khi enemy đã chết + collider nổ đang bật
        if (isDead && explosionCollider != null && explosionCollider.enabled && !hasExploded)
        {
            if (other.CompareTag("Player"))
            {
                hasExploded = true;
                HealthManager playerHP = other.GetComponent<HealthManager>();
                if (playerHP != null)
                {
                    playerHP.TakeDamage(explosionDamage);
                    Debug.Log("Explosion damaged player!");
                }
            }
        }
        else
        {
            // Xử lý va chạm bình thường khi chưa chết
            if (other.CompareTag("PlayerHitbox"))
            {
                if (Time.time - lastDamageTime >= damageCooldown)
                {
                    TakeDamage(5);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
