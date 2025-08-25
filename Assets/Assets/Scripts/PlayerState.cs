// PlayerState.cs
using UnityEngine;

// Base class for player states. Add FixedUpdate virtual so states can override physics logic.
public abstract class PlayerState
{
    protected PlayerController player;

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void HandleInput() { }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }
}
