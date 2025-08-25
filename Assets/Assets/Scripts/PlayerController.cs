// PlayerController.cs
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Health")]
    public PlayerHealth healthManager;
    public Slider healthSlider; // optional: assign to player's UI slider
    public Text healthText;     // optional: assign to player's UI text

    // internal state & flags
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isFacingRight = true;
    private PlayerState currentState;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (healthManager == null) healthManager = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        // If healthManager exists and slider/text assigned in controller, forward them to healthManager
        if (healthManager != null)
        {
            if (healthSlider != null) healthManager.healthSlider = healthSlider;
            if (healthText != null) healthManager.healthText = healthText;
        }

        // start in Idle
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        // ground check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        currentState?.HandleInput();
        currentState?.Tick();

        // debug/test keys
        if (Input.GetKeyDown(KeyCode.K))
        {
            // test damage
            if (healthManager != null) healthManager.TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (healthManager != null) healthManager.Heal(15);
        }
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == null) return;
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Helper called by states to move player using physics
    public void MovePhysics(float horizontalInput)
    {
        if (rb == null) return;
        Vector2 vel = rb.linearVelocity;
        vel.x = horizontalInput * moveSpeed;
        rb.linearVelocity = vel;

        Flip(horizontalInput);

        // set animator params if present
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            animator.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.01f);
        }
    }

    // Flip sprite by changing localScale.x
    public void Flip(float horizontalInput)
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            isFacingRight = true;
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            isFacingRight = false;
        }
    }

    // Fall multiplier for better jump feel
    public float GetFallMultiplier()
    {
        return 2.5f;
    }

    // Public heal/damage wrappers for UI buttons or external systems
    public void ApplyDamage(int dmg)
    {
        if (healthManager != null) healthManager.TakeDamage(dmg);
    }

    public void ApplyHeal(int amount)
    {
        if (healthManager != null) healthManager.Heal(amount);
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
}

/*
Hookup instructions (quick):
1) Put these scripts into Assets/Scripts/ (no namespaces).
2) On your Player GameObject:
   - Add Rigidbody2D and Animator components (if not present).
   - Add PlayerController and PlayerHealth components.
   - Assign Rigidbody2D and Animator to the PlayerController fields or let Awake auto-assign.
   - Create an empty child GameObject placed at player's feet, assign it to groundCheck.
   - Set groundLayer to the layer(s) used by your ground tiles.
3) Create a UI Canvas with a Slider for health and optional Text for numbers.
   - Assign the Slider and Text to PlayerController.healthSlider and healthText, or directly to PlayerHealth.healthSlider/healthText.
4) Play and test:
   - Press Horizontal (A/D or Left/Right) to run.
   - Press W to jump.
   - Press K to take 10 damage for testing, J to heal for testing.
*/
