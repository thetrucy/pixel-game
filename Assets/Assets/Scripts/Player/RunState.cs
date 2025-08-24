using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.animator != null) player.animator.SetBool("isRunning", true);
    }

    public override void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) == 0)
        {
            player.ChangeState(new IdleState(player));
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

    public override void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (player.rb != null)
        {
            if (moveInput != 0)
            {
                player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);
                player.Flip(moveInput);
            }
            else
            {
                player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y); // Chống trượt
            }
        }
    }
}