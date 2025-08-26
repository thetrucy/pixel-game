using UnityEngine;

public class JumpAttackState : PlayerState
{
    private float lockDuration = 0.4f;  // thời gian khóa hướng
    private float timer;
    private float lockedDirection; // -1 (trái), +1 (phải)

    public JumpAttackState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        timer = 0f;

        lockedDirection = player.isFacingRight ? 1f : -1f;

        if (player.rb != null)
        {
            player.rb.linearVelocity = new Vector2(lockedDirection * player.moveSpeed, player.rb.linearVelocity.y);
        }

        // Play animation
        if (player.animator != null)
        {
            player.animator.Play("Player_JAA");
        }

        Debug.Log("Enter JumpAttack State");
    }

    public override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(new DashState(player));
            return;
        }
        if (player.isGrounded)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(moveInput) > 0)
            {
                player.Flip(moveInput);
                player.ChangeState(new RunState(player));
            }
            else
            {
                player.ChangeState(new IdleState(player));
            }
        }
    }

    public override void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer < lockDuration)
            return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (player.rb != null)
        {
            float xVel = moveInput * player.moveSpeed;
            player.rb.linearVelocity = new Vector2(xVel, player.rb.linearVelocity.y);

            if (Mathf.Abs(moveInput) > 0)
            {
                player.Flip(moveInput);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit JumpAttack State");
    }
}
