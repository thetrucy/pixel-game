using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public HealthManager healthManager;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    [Range(1f, 5f)]
    public float fallMultiplier = 5f;

    public bool isFacingRight = true;
    public bool isGrounded = false;

    private PlayerState currentState;
    public GameObject hitboxJAA;

    public void EnableJumpAttackHitbox() => hitboxJAA.SetActive(true);
    public void DisableJumpAttackHitbox() => hitboxJAA.SetActive(false);


    void Start()
    {
        if (rb == null || animator == null || healthManager == null)
        {
            Debug.LogError("Missing component: Rigidbody2D, Animator, or HealthManager!");
            enabled = false;
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
        currentState = new IdleState(this); // Khởi tạo chắc chắn với IdleState
        Debug.Log("Starting state: " + currentState.GetType().Name);
    }

    void Update()
    {
        ComboAttack combo = GetComponent<ComboAttack>();

        if (combo != null && combo.IsAttacking() && Input.GetKeyDown(KeyCode.LeftShift))
        {
            combo.CancelAttack();
            ChangeState(new DashState(this));
            return;
        }

        if (combo != null && combo.IsAttacking())
        {
            return;
        }

        if (currentState != null)
        {
            currentState.HandleInput();
            currentState.Update();
        }
    }

    void FixedUpdate()
    {
        ComboAttack combo = GetComponent<ComboAttack>();
        if (combo != null && combo.IsAttacking())
        {
            // Ngăn di chuyển khi tấn công
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (currentState != null) currentState.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        if (currentState != null) currentState.Enter();
        else Debug.LogError("Failed to change state: newState is null");
        Debug.Log("Changed to state: " + (newState != null ? newState.GetType().Name : "Null"));
    }

    public void Flip(float moveInput)
    {
        if (moveInput > 0 && !isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            isFacingRight = true;
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            isFacingRight = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                Debug.Log("Collision detected with " + collision.gameObject.name + ", isGrounded: true");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (isGrounded && rb.linearVelocity.y < -0.1f)
        {
            isGrounded = false;
            Debug.Log("Exited collision with " + collision.gameObject.name + ", isGrounded: false");
        }
    }

    public float GetFallMultiplier()
    {
        return fallMultiplier;
    }
}