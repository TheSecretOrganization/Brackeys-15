using Godot;

public partial class GoEnd : Area2D
{

    [Signal] public delegate void ObjectEndEventHandler();
    [Export] private Label _hint;

    private bool _inArea;

    public override void _Ready()
    {
        BodyEntered += EnterArea;
        BodyExited += LeaveArea;
    }

    private void EnterArea(Node2D body)
    {
        if (body is not Player) return;

        PickItem();
    }

    private void PickItem()
    {
        QueueFree();
        EmitSignalObjectEnd();
    }

    private void LeaveArea(Node2D body)
    {
        if (body is not Player) return;
        _hint.Text = "";
        _inArea = false;
    }

}
