using Godot;

public partial class Pickable : Area2D
{

    [Signal] public delegate void ObjectPickedEventHandler();
    [Export] private Label _hint;

    private bool _inArea;

    public override void _Ready()
    {
        BodyEntered += EnterArea;
        BodyExited += LeaveArea;
    }
    
    public override void _Process(double delta)
    {
        if (_inArea && Input.IsActionJustPressed("interact"))
            PickItem();
    }

    private void PickItem()
    {
        QueueFree();
        EmitSignalObjectPicked();
    }

    private void EnterArea(Node2D body)
    {
        if (body is not Player) return;
        _hint.Text = "Press E to collect";
        _inArea = true;
    }

    private void LeaveArea(Node2D body)
    {
        if (body is not Player) return;
        _hint.Text = "";
        _inArea = false;
    }
    
}
