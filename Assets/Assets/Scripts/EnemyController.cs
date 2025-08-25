using UnityEngine;
using System.Collections;

public class EnemyController : HealthBase
{
    [Header("Stats")]
    public int damage = 20;
    public float walkSpeed = 2f;
    public float walkTime = 2f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float detectionRange = 5f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    private Collider2D enemyCol;
    private bool isFacingRight = true;
    private float walkTimer;
    private bool isDashing = false;
    private bool canDamage = false;
    private Vector3 initialScale; // Lưu scale ban đầu

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyCol = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        walkTimer = walkTime;
        initialScale = new Vector3(7.5f, 7.5f, 1f); // Đặt scale ban đầu là 7.5 cho x và y
        transform.localScale = initialScale; // Áp dụng scale ban đầu

        // Chỉ bỏ va chạm với Player
        if (player != null && enemyCol != null)
        {
            Collider2D playerCol = player.GetComponent<Collider2D>();
            if (playerCol != null)
            {
                Physics2D.IgnoreCollision(enemyCol, playerCol, true);
            }
        }
    }

    private void Update()
    {
        if (GetCurrentHealth() <= 0) return;

        if (!isDashing)
        {
            if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
            {
                FacePlayer();
                StartCoroutine(Dash());
            }
            else
            {
                Patrol();
            }
        }

        if (animator != null)
            animator.SetBool("IsDashing", isDashing);
    }

    private void Patrol()
    {
        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * walkSpeed, rb.linearVelocity.y);

        walkTimer -= Time.deltaTime;
        if (walkTimer <= 0)
        {
            Flip();
            walkTimer = walkTime;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDamage = true;

        Vector2 dashDir = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;
        rb.linearVelocity = dashDir * dashSpeed;

        if (animator != null)
            animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        canDamage = false;
        isDashing = false;

        if (animator != null)
            animator.SetBool("IsDashing", false);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        float scaleX = isFacingRight ? Mathf.Abs(initialScale.x) : -Mathf.Abs(initialScale.x); // Chỉ thay đổi dấu x, giữ độ lớn
        transform.localScale = new Vector3(scaleX, initialScale.y, initialScale.z); // Giữ nguyên y và z
    }

    private void FacePlayer()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && !isFacingRight)
            Flip();
        else if (player.position.x < transform.position.x && isFacingRight)
            Flip();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }

    protected override void Die()
    {
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("IsDead"); // Sử dụng SetTrigger thay vì SetBool

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("Enemy died!", gameObject);
        Destroy(gameObject, 0.5f);
    }
}