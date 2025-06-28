using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    // For more robust ground checking
    public Transform groundCheck; // Assign an empty GameObject as a child slightly below the player's feet
    public LayerMask groundLayer; // Set this in the Inspector to your "Ground" layer(s)
    public float groundCheckRadius = 0.2f; // Adjust this radius as needed for your player size

    public bool isFacingRight = true; // Biến để theo dõi hướng của nhân vật

    // Renamed for clarity: this now represents if the player is currently touching the ground
    private bool _isCurrentlyGrounded = false; // Using a private backing field

    void Start()
    {
        // Get components if not already assigned in Inspector
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();

        // Ensure groundCheck is assigned
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck Transform is not assigned! Jumping and grounding logic will not work correctly.", this);
        }
    }

    void Update()
    {
        // *** NEW: Continuous Ground Check ***
        // This is the most important change. It constantly updates _isCurrentlyGrounded.
        _isCurrentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // *** Update Animator's isJumping parameter based on current ground state ***
        // This will now correctly transition into "Jump" when airborne and out when grounded.
        animator.SetBool("isJumping", !_isCurrentlyGrounded);

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

        // *** Jumping Logic ***
        // Only allow jump if currently grounded.
        // No need to manually set _isCurrentlyGrounded = false here; the ground check handles it.
        if (Input.GetKeyDown(KeyCode.W) && _isCurrentlyGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // animator.SetBool("isJumping", true); // No need to set here, handled by the continuous check above
        }

        Flip();

        if (Mathf.Abs(moveInput) > 0.01f) // Use a small epsilon to avoid floating point issues near zero
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("AA1");
        }

        if (Input.GetKeyDown(KeyCode.S)) 
        {
            animator.SetTrigger("S"); // Renamed to "HealTrigger" or similar in Animator
        }

        // Ham Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && _isCurrentlyGrounded) // Only allow dash if grounded
        {
            animator.SetTrigger("Dash");
            Dash();
        }
    }



    void Flip()
    {
        // Quay nhân vật theo hướng di chuyển
        if (rb.linearVelocity.x > 0.01f && !isFacingRight) 
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // đảm bảo scale.x dương
            transform.localScale = scale;
            isFacingRight = true;
        }
        else if (rb.linearVelocity.x < -0.01f && isFacingRight) // Use Rigidbody velocity
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // đảm bảo scale.x âm
            transform.localScale = scale;
            isFacingRight = false;
        }
    }

    void Dash()
    {
        float dashSpeed = 10f; // bạn có thể chỉnh số này
        float dashDuration = 0.2f;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = dashDirection * dashSpeed; // Instantaneous velocity change for dash


        Invoke(nameof(StopDash), dashDuration);
    }

    void StopDash()
    {
        
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}