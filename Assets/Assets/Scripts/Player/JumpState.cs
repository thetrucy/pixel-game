using UnityEngine;

public class JumpState : PlayerState
{
    private bool applyJumpForce;
    private bool dashAble;

    // Thêm tham số applyJumpForce
    public JumpState(PlayerController player, bool applyJumpForce = true, bool dashAble = true) : base(player)
    {
        this.applyJumpForce = applyJumpForce;
        this.dashAble = dashAble;
    }

    public override void Enter()
    {
        // Chỉ thêm lực nhảy khi applyJumpForce = true
        if (applyJumpForce && player.rb != null)
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
        }

        player.isGrounded = false;

        // Bật animation Jump
        if (player.animator != null)
        {
            player.animator.SetBool("isRunning", false);
            player.animator.SetBool("isJumping", true);
        }
    }

    public override void HandleInput()
    {
        // Khi chạm đất → Idle hoặc Run
        if (player.isGrounded)
        {
            dashAble = true;
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0)
                player.ChangeState(new RunState(player));
            else
                player.ChangeState(new IdleState(player));
        }

        // Dash khi đang ở trên không
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashAble)
        {
            dashAble = false;
            player.ChangeState(new DashState(player));
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(new JumpAttackState(player));
        }
    }

    public override void FixedUpdate()
    {
        if (player.rb != null)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            // Cho phép điều khiển ngang khi nhảy
            player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);

            // Lật hướng sprite
            player.Flip(moveInput);
        }
    }

    public override void Exit()
    {
        // Reset flag Jump khi thoát state
        if (player.animator != null)
        {
            player.animator.SetBool("isJumping", false);
        }
    }
}
