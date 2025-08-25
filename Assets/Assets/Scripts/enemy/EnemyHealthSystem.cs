using UnityEngine;
using System.Collections;

public class EnemyHealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public float damageCooldown = 0.35f;
    private float lastDamageTime = -Mathf.Infinity;

    private SpriteRenderer spriteRenderer;

    public GameObject hitEffectPrefab;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(5);
                lastDamageTime = Time.time;
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Spawn hiệu ứng hit
        if (hitEffectPrefab != null)
        {
            // Random góc xoay (chỉ xoay Z cho 2D)
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            // Tạo hiệu ứng
            GameObject effect = Instantiate(
                hitEffectPrefab,
                transform.position + new Vector3(0, 4.5f, 0),
                randomRotation
            );

            // Tự hủy sau khi animation chạy xong
            float lifetime = 0.5f; // mặc định 0.5s nếu prefab không có animator
            Animator anim = effect.GetComponent<Animator>();
            if (anim != null)
            {
                // lấy thời gian clip đầu tiên trong Animator
                AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    lifetime = clipInfo[0].clip.length;
                }
            }
            Destroy(effect, lifetime);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
