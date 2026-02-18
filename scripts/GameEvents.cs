using Godot;
using System;


public partial class GameEvents : Node
{
    [Signal] public delegate void CheckpointActivatedEventHandler(Vector2 position);

    public static GameEvents Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
}
