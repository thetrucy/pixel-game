using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Damage Settings")]
    public float damageCooldown = 0.35f;
    private float lastDamageTime = -Mathf.Infinity;

    public event System.Action OnDie;

    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;
    public string hitEffectSortingLayer = "Foreground";
    public int hitEffectOrderInLayer = 10;
    public float defaultEffectLifetime = 0.5f;

    private SpriteRenderer spriteRenderer;

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

        SpawnHitEffect();

        // Camera shake
        if (CameraShake.Instance != null)
            StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.05f));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;

        // Spawn tại vị trí enemy, z = 0
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0f);

        // Random xoay Z
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        GameObject effect = Instantiate(hitEffectPrefab, spawnPos, randomRotation);

        // Set sorting layer nếu có SpriteRenderer
        SpriteRenderer sr = effect.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = hitEffectSortingLayer;
            sr.sortingOrder = hitEffectOrderInLayer;
        }

        // Tự hủy sau khi animation chạy xong
        float lifetime = defaultEffectLifetime;
        Animator anim = effect.GetComponent<Animator>();
        if (anim != null)
        {
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                lifetime = clipInfo[0].clip.length;
            }
        }

        Destroy(effect, lifetime);
    }

    protected virtual void Die()
    {
        OnDie?.Invoke();
    }
}
