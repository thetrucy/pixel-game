using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.rb != null) player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
        player.isGrounded = false;
        if (player.animator != null)
        {
            //player.animator.SetBool("isRunning", false);
            //player.animator.SetBool("isJumping", true);
        }
    }

    public override void HandleInput()
    {
        if (player.isGrounded)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0)
            {
                player.ChangeState(new RunState(player));
            }
            else
            {
                player.ChangeState(new IdleState(player));
            }
        }
        if (Input.GetKeyDown(KeyCode.W) && player.isGrounded)
        {
            player.ChangeState(new JumpState(player));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.isGrounded)
        {
            player.ChangeState(new DashState(player));
        }
        // Không cần xử lý tấn công ở đây, để ComboAttack xử lý
    }

    public override void Exit()
    {
        if (player.animator != null)
        {
            //player.animator.SetBool("isJumping", false);
        }
    }

    public override void FixedUpdate()
    {
        if (player.rb != null)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);
            player.Flip(moveInput);

            if (player.rb.linearVelocity.y < 0)
            {
                //if (player.animator != null) player.animator.SetBool("isFalling", true); // Thêm trạng thái rơi
                player.rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (player.GetFallMultiplier() - 1) * Time.fixedDeltaTime;
            }
            else if (player.rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W))
            {
                player.rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (player.GetFallMultiplier() - 1) * Time.fixedDeltaTime;
            }
            else
            {
                //if (player.animator != null) player.animator.SetBool("isFalling", false); // Reset khi nhảy lên
            }
        }
    }
}