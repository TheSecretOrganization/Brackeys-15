using Godot;
using System;

public partial class Checkpoint : Area2D
{
    public override void _Ready()
    {
        BodyEntered += _onBodyEntered;
    }

    private void _onBodyEntered(Node2D body)
    {
        if (body is Player)
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.CheckpointActivated, GlobalPosition);
    }
}
