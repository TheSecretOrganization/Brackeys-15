using Godot;
using System;

public partial class Manager : Node
{
    private void OnBodyFallOutOfMap(Node2D body)
    {
        if (body is Player player) player.Respawn();
    }
}
