using Godot;
using System;

public partial class DialogueEvent : Node
{

    [Export] private Player _player;
    [Export] private string[] _dialogue;

    private void ShowDialogue()
    {
        _player.Dialogue.Show(_dialogue);
    }

}
