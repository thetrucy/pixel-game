using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void HandleInput() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}