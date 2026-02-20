using Godot;

public partial class Dialogue : Area2D
{

    [Export] private string[] _texts;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player) return;
        QueueFree();
        player.Dialogue.Show(_texts);
    }

}
