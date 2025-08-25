using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float lastClickTime;
    private float maxComboDelay = 1f;

    [Header("Attack Settings")]
    public int[] attackDamage = { 10, 15, 20 }; // Sát thương cho từng bước combo (AA1, AA2, AA3)
    public Collider2D attackCollider; // Gán collider trigger cho vùng đánh trong Inspector

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name + "! Please assign an Animator component.");
            enabled = false;
            return;
        }
        if (attackCollider == null)
        {
            Debug.LogError("Attack Collider not assigned on " + gameObject.name + "! Please assign a trigger collider.");
            enabled = false;
            return;
        }
        attackCollider.enabled = false; // Tắt collider ban đầu
    }

    void Update()
    {
        // Xử lý input tấn công độc lập cho cả mặt đất và không trung
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        // Reset combo nếu quá thời gian
        if (Time.time - lastClickTime > maxComboDelay)
        {
            comboStep = 0;
            attackCollider.enabled = false; // Tắt collider khi reset
        }
    }

    public void OnClick()
    {
        lastClickTime = Time.time;
        comboStep++;

        if (animator != null)
        {
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null)
            {
                if (!pc.isGrounded) Debug.Log("Attacking in air, comboStep: " + comboStep);
                else Debug.Log("Attacking on ground, comboStep: " + comboStep);
            }

            if (comboStep == 1)
            {
                animator.SetTrigger("AA1");
                Debug.Log("Triggering AA1 at " + Time.time);
            }
            else if (comboStep == 2)
            {
                animator.SetTrigger("AA2");
                Debug.Log("Triggering AA2 at " + Time.time);
            }
            else if (comboStep == 3)
            {
                animator.SetTrigger("AA3");
                Debug.Log("Triggering AA3 at " + Time.time);
            }
            else
            {
                ResetCombo(); // Reset nếu vượt quá 3 combo
            }
        }
        else
        {
            Debug.LogError("Animator is null in ComboAttack.OnClick");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCollider.enabled && other.CompareTag("Enemy"))
        {
            HealthBase health = other.GetComponent<HealthBase>();
            if (health != null)
            {
                int damage = GetCurrentComboDamage();
                health.TakeDamage(damage);
                Debug.Log("Hit enemy " + other.name + " for " + damage + " damage at combo step " + comboStep);
            }
        }
    }

    private int GetCurrentComboDamage()
    {
        if (comboStep > 0 && comboStep <= attackDamage.Length)
        {
            return attackDamage[comboStep - 1]; // Trả về sát thương theo bước combo (0-based index)
        }
        return 0;
    }

    public void EnableAttackCollider()
{
    if (attackCollider != null)
    {
        attackCollider.enabled = true;
        Debug.Log("Attack collider ENABLED at " + Time.time + " for combo step " + comboStep);
    }
    else
    {
        Debug.LogError("AttackCollider is NULL!");
    }
}

public void DisableAttackCollider()
{
    if (attackCollider != null)
    {
        attackCollider.enabled = false;
        Debug.Log("Attack collider DISABLED at " + Time.time + " for combo step " + comboStep);
    }
}

    public void ResetCombo()
    {
        comboStep = 0;
        if (attackCollider != null) attackCollider.enabled = false;
        Debug.Log("Combo reset at " + Time.time);
    }

    // Vẽ gizmo để hiển thị phạm vi attack (dùng cho debug, dựa trên collider)
    private void OnDrawGizmosSelected()
    {
        if (attackCollider != null)
        {
            Gizmos.color = Color.red;
            if (attackCollider is BoxCollider2D box)
            {
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
            }
            else if (attackCollider is CircleCollider2D circle)
            {
                Gizmos.DrawWireSphere(circle.bounds.center, circle.radius);
            }
        }
    }
}