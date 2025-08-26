using UnityEngine;

namespace Enemies.Aladin
{
    public class AladinHealth : EnemyHealthSystem
    {
        private Animator animator;
        private bool isDead = false;
        private bool hasExploded = false;
        private float lastDamageTime = -Mathf.Infinity;

        [Header("Explosion Settings")]
        public Collider2D explosionCollider;
        public int explosionDamage = 20;


        public bool isDead1() => isDead;
        protected override void Start()
        {
            maxHealth = 15;
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator not found for AladinHealth!");
            }

            if (explosionCollider != null)
            {
                explosionCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("ExplosionCollider not assigned for AladinHealth!");
            }

            base.Start();
        }

        protected override void Die()
        {
            if (isDead) return;
            isDead = true;

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            AladinChase chase = GetComponent<AladinChase>();
            if (chase != null)
            {
                chase.enabled = false;
            }

            AladinAttack attack = GetComponent<AladinAttack>();
            if (attack != null)
            {
                attack.enabled = false;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Hủy đăng ký với EnemyManager
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.UnregisterEnemy();
            }
            else
            {
                Debug.LogWarning("EnemyManager instance not found for AladinHealth!");
            }

            // Sau 1s thì phát nổ
            Invoke(nameof(Explode), 1f);

            base.Die();

            Destroy(gameObject, 1.3f);
        }

        private void Explode()
        {
            if (explosionCollider == null)
            {
                Debug.LogWarning("ExplosionCollider not found for AladinHealth!");
                return;
            }

            hasExploded = false;
            explosionCollider.enabled = true;
            explosionCollider.isTrigger = true;

            Invoke(nameof(DisableExplosion), 0.2f);
        }

        private void DisableExplosion()
        {
            if (explosionCollider != null)
            {
                explosionCollider.enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead && explosionCollider != null && explosionCollider.enabled && !hasExploded)
            {
                if (other.CompareTag("Player"))
                {
                    hasExploded = true;
                    HealthManager playerHP = other.GetComponent<HealthManager>();
                    if (playerHP != null)
                    {
                        playerHP.TakeDamage(explosionDamage);
                        Debug.Log($"Aladin explosion caused {explosionDamage} damage to player!");
                    }
                }
            }
            else if (!isDead && other.CompareTag("PlayerHitbox"))
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