using UnityEngine;
using System.Collections; // Thư viện xài Coroutines

public class PlayerController : MonoBehaviour
{
public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public bool isFacingRight = true; // Biến để theo dõi hướng của nhân vật
    public bool canMove = true; // Controls whether player can move or take input
    private bool isHealing = false; // Flag to prevent multiple heals
    private bool isAttacking = false; // Flag to prevent actions while attacking
    private bool isGrounded = false; // Checks if player is on the ground
    private int comboStep = 0; // Current step in the combo sequence (0 = ready for AA1)
    private float lastAttackTime; // Time when the last attack was triggered
    public float maxComboDelay = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra heal để set canMove
        if (Input.GetKeyDown(KeyCode.S) && !isHealing && isGrounded && !isAttacking)
        {
            StartCoroutine(HealRoutine());
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isHealing && !isAttacking)
        {
            HandleComboAttack();
            return;
        }
        // Nếu kh có hành động nào ngăn canMove
        if (canMove && !isAttacking && !isHealing)
        {
            float moveInput = 0f;

            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
            }

            Flip(); // Flip logic based on current movement input

            // Update running animation bool
            if (Mathf.Abs(moveInput) != 0f)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }


            // Ham Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            {
                animator.SetTrigger("Dash");
                Dash();
            }
        }
        else // Khi không di chuyển. đảm bảo canMove
        {
            // Stop horizontal movement
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator.SetBool("isRunning", false); // Đảm bảo cho animation idle hoạt động
        }
        if (comboStep != 0 && Time.time - lastAttackTime > maxComboDelay)
        {
            comboStep = 0; // Reset combo if timeout
        }
    }

    void Flip()
    {
        // Quay nhân vật theo hướng di chuyển
        float currentMoveInput = Input.GetAxisRaw("Horizontal");

        if (currentMoveInput > 0 && !isFacingRight) // Moving right
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // đảm bảo scale.x dương
            transform.localScale = scale;
            isFacingRight = true;
        }
        else if (currentMoveInput < 0 && isFacingRight) // Moving left
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // đảm bảo scale.x âm
            transform.localScale = scale;
            isFacingRight = false;
        }
    }
    // Check ground khi chạm đất
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }
    }

    // Check ground khi rơi
    void OnCollisionExit2D(Collision2D collision)
    {
        if (isGrounded && rb.linearVelocity.y < -0.1f)
        {
            isGrounded = false;
        }
    }

    void Dash()
    {
        float dashSpeed = 20f; // có thể chỉnh số này
        float dashDuration = 0.4f;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = dashDirection * dashSpeed;

        // Changed from Invoke to Coroutine for better control
        StartCoroutine(StopDashRoutine(dashDuration));
    }

    // Coroutine for stopping dash
    IEnumerator StopDashRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero; // dừng lại sau khi dash
    }

    // Coroutine for healing
    private IEnumerator HealRoutine()
    {
        isHealing = true; // Chống 2 heal chồng lên nhau
        canMove = false;  // Ngăn không cho di chuyển trong khi heal

        Debug.Log("Healing started: Player cannot move.", this);

        // Heal Animation
        if (animator != null)
        {
            animator.SetTrigger("Heal");
        }
        // Chờ hết heal
        yield return new WaitForSeconds(1.3f);

        // Bật canMove
        canMove = true;
        isHealing = false;
        Debug.Log("Healing finished: Player can move again.", this);
    }

void HandleComboAttack()
    {
        lastAttackTime = Time.time; // Update the last attack time *before* incrementing comboStep for accurate timing
        if (comboStep == 0 || (Time.time - lastAttackTime > maxComboDelay && comboStep != 0)) // Check if starting fresh
        {
            comboStep = 1; // Start with AA1
        }
        else
        {
            comboStep++; // Otherwise, increment for the next step in the combo chain
        }
            // Trigger the corresponding animation
        if (comboStep == 1)
        {
            animator.SetTrigger("AA1");
            Debug.Log("Combo Attack 1 (AA1)");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("AA2");
            Debug.Log("Combo Attack 2 (AA2)");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("AA3");
            Debug.Log("Combo Attack 3 (AA3) - Combo Finished");
        }

        // Set isAttacking to true the moment ANY attack is initiated
        isAttacking = true;
    }

    // --- Animation Events (called from Animator) ---

    // Called at the point in the animation where player can input next action or move.
    public void OnAttackAnimationEnd()
    {
        isAttacking = false; // Player is no longer in an attack animation
        Debug.Log("OnAttackAnimationEnd: Player can now move/start next action.");
    }

    // Called specifically from the AA3 animation clip at its end to fully reset the combo
    public void ResetCombo()
    {
        comboStep = 0;
        Debug.Log("ComboStep reset via AA3 Animation Event.");
    }

}
