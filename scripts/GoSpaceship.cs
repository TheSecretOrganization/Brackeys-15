using Godot;

public partial class GoSpaceship : Area2D
{

    [Signal] public delegate void ObjectSpaceshipEventHandler();
    [Export] private Label _hint;

    private bool _inArea;

    public override void _Ready()
    {
        BodyEntered += EnterArea;
        BodyExited += LeaveArea;
    }

    private void PickItem()
    {
        QueueFree();
        EmitSignalObjectSpaceship();
    }

    private void EnterArea(Node2D body)
    {
        if (body is not Player) return;

        PickItem(); // Trigger immediately on touch
    }

    private void LeaveArea(Node2D body)
    {
        if (body is not Player) return;
        _hint.Text = "";
        _inArea = false;
    }

}
