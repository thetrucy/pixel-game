// HealState.cs
using UnityEngine;
using System.Collections;

public class HealState : PlayerState
{
    private float healTime = 1.2f;
    private int healAmount = 20;

    public HealState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player == null) return;
        if (player.animator != null) player.animator.SetTrigger("Heal");
        player.StartCoroutine(HealCoroutine());
    }

    private IEnumerator HealCoroutine()
    {
        // block movement during heal (controller can disable input if needed)
        yield return new WaitForSeconds(healTime);
        if (player != null && player.healthManager != null)
        {
            player.healthManager.Heal(healAmount);
        }
        // return to appropriate state
        float h = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(h) > 0.01f) player.ChangeState(new RunState(player));
        else player.ChangeState(new IdleState(player));
    }
}
