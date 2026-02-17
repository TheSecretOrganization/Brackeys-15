using Godot;
using System;

public partial class Void : Area2D
{
    static public void OnBodyEntered(Node2D body)
    {
        if (body is IKillable iKillable) iKillable.Die();
    }
}