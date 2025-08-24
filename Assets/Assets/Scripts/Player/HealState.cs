using UnityEngine;
using System.Collections;

public class HealState : PlayerState
{
    public HealState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        if (player.animator != null) player.animator.SetTrigger("Heal");
        player.StartCoroutine(HealRoutine());
    }

    private IEnumerator HealRoutine()
    {
        Debug.Log("Healing started: Player cannot move.", player);
        yield return new WaitForSeconds(1.3f);
        if (player.healthManager != null) player.healthManager.Heal(20);
        Debug.Log("Healing finished: Player can move again.", player);
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