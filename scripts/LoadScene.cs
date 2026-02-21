using Godot;
using System;

public partial class LoadScene : Node
{

    [Export] private PackedScene _scene;

    private void ChangeScene()
    {
        GetTree().ChangeSceneToPacked(_scene);
    }

}
