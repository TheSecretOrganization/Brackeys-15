using Godot;
using System;

public partial class MainMenu : Control
{

    [Export] private PackedScene _level;

    private void StartButton()
    {
        GetTree().ChangeSceneToPacked(_level);
    }

    private void QuitButton()
    {
        GetTree().Quit();
    }

}
