using Godot;

using Godot;

public partial class OutroText : Node2D
{
    [Export] private Label _label;

    public void PrintOutro()
    {
        _label.Text = "Note: Poochie Died on the way back to his home planet";
    }
}
