using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.animator != null) player.animator.SetBool("isRunning", false);
        if (player.rb != null) player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
    }

    public override void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) > 0)
        {
            player.ChangeState(new RunState(player));
        }
        if (Input.GetKeyDown(KeyCode.W) && player.isGrounded)
        {
            player.ChangeState(new JumpState(player));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.isGrounded)
        {
            player.ChangeState(new DashState(player));
        }
        if (Input.GetKeyDown(KeyCode.S) && player.isGrounded)
        {
            player.ChangeState(new HealState(player));
        }
    }
}