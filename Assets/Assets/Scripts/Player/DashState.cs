using UnityEngine;
using System.Collections;

public class DashState : PlayerState
{
    private float dashSpeed = 20f;
    private float dashDuration = 0.4f;

    public DashState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.animator != null) player.animator.SetTrigger("Dash");
        Vector2 dashDirection = player.isFacingRight ? Vector2.right : Vector2.left;
        if (player.rb != null) player.rb.linearVelocity = dashDirection * dashSpeed;
        player.StartCoroutine(StopDashRoutine());
    }

    private IEnumerator StopDashRoutine()
    {
        yield return new WaitForSeconds(dashDuration);
        if (player.rb != null) player.rb.linearVelocity = Vector2.zero;
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
}