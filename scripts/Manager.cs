using Godot;
using System;

public partial class Manager : Node
{
    private Vector2 _currentRespawnPosition = new(0, 0);

    public override void _Ready()
    {
        base._Ready();
        _currentRespawnPosition = GetNode<Player>("Player").GlobalPosition;
        GameEvents.Instance.CheckpointActivated += _onCheckPointActivated;
    }

    private void _onCheckPointActivated(Vector2 position)
    {
        _currentRespawnPosition = position;
    }

    private void _onBodyFallOutOfMap(Node2D body)
    {
        if (body is Player player) player.Respawn(_currentRespawnPosition);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && GameEvents.Instance != null)
        {
            GameEvents.Instance.CheckpointActivated -= _onCheckPointActivated;
        }
        base.Dispose(disposing);
    }
}
