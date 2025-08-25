// IdleState.cs
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.animator != null)
        {
            player.animator.SetBool("isRunning", false);
            player.animator.SetBool("isJumping", false);
            player.animator.SetBool("isFalling", false);
        }
    }

    public override void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(h) > 0.01f)
        {
            player.ChangeState(new RunState(player));
            return;
        }
        if (Input.GetKeyDown(KeyCode.W) && player.isGrounded)
        {
            player.ChangeState(new JumpState(player));
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.ChangeState(new HealState(player));
        }
    }
}
