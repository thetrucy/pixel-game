// RunState.cs
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
        float h = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(h) < 0.01f)
        {
            player.ChangeState(new IdleState(player));
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
            return;
        }
    }

    public override void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        player.MovePhysics(h);
    }
}
