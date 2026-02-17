using Godot;
using System;

public partial class PlayerSpawner : Node2D
{
    [Export] private PackedScene _playerScene;
    [Export] private float _timeout = 0.2f;

    public override void _Ready()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (_playerScene == null) return;

        var player = _playerScene.Instantiate<Player>();
        player.GlobalPosition = GlobalPosition;
        player.TreeExited += () => { GetTree().CreateTimer(_timeout).Timeout += SpawnPlayer; };
        GetParent().CallDeferred(Node.MethodName.AddChild, player);
    }
}