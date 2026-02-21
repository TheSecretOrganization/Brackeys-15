using Godot;

public partial class ElectricalWall : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        if (body is IKillable killable) killable.Die();
    }
}
