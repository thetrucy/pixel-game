using UnityEngine;
using System.Collections;
using System.Linq;

public class DashState : PlayerState
{
    private float dashSpeed = 20f;
    private float dashDuration = 0.4f;
    private float originalGravity;
    private float originalAnimatorSpeed;
    private string dashClipName = "Dash";
    private bool isDashing = false;

    public DashState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        // Lưu gravity và animator speed gốc
        originalGravity = player.rb.gravityScale;
        originalAnimatorSpeed = player.animator != null ? player.animator.speed : 1f;

        // Tạm tắt gravity
        player.rb.gravityScale = 0f;

        // Tắt isJumping để animation Dash không bị blend từ Jump
        if (player.animator != null)
        {
            player.animator.SetBool("isJumping", false);

            // Scale tốc độ animation Dash theo duration
            var dashClip = player.animator.runtimeAnimatorController.animationClips
                .FirstOrDefault(c => c.name == dashClipName);
            if (dashClip != null)
            {
                player.animator.speed = dashClip.length / dashDuration;
            }

            // Trigger Dash animation ngay
            player.animator.SetTrigger("Dash");
        }

        // Bắt đầu dash movement
        isDashing = true;
        Vector2 dashDirection = player.isFacingRight ? Vector2.right : Vector2.left;
        player.rb.linearVelocity = dashDirection * dashSpeed;

        // Start coroutine để kết thúc dash
        player.StartCoroutine(StopDashRoutine());
    }

    // Nếu muốn sync animation kéo movement theo frame
    public void OnAnimatorMove()
    {
        if (isDashing && player.animator != null)
        {
            Vector2 delta = new Vector2(player.animator.deltaPosition.x, 0f);
            player.rb.MovePosition(player.rb.position + delta);
        }
    }

    private IEnumerator StopDashRoutine()
    {
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Khôi phục gravity
        player.rb.gravityScale = originalGravity;

        // Reset vận tốc ngang
        player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y);

        // Reset animator speed
        if (player.animator != null)
            player.animator.speed = originalAnimatorSpeed;

        // Chuyển state về Jump hoặc Idle/Run
        if (!player.isGrounded)
        {
            // Đảm bảo animation Jump được bật ngay khi thoát Dash
            if (player.animator != null)
            {
                player.animator.ResetTrigger("Dash");
                player.animator.SetBool("isJumping", true);
            }

            player.ChangeState(new JumpState(player, false, false));
        }
        else
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0)
                player.ChangeState(new RunState(player));
            else
                player.ChangeState(new IdleState(player));
        }
    }

}
