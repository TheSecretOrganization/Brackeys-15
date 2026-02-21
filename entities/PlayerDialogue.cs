using Godot;
using System;

public partial class PlayerDialogue : CenterContainer
{

    [Export] private Label _label;
    [Export] private Timer _timer;

    private int _index;
    private string[] _textQueue;

    public override void _Ready()
    {
        _timer.Timeout += ClearText;
    }

    public void Show(string[] texts)
    {
        _index = 0;
        _textQueue = texts;
        IterateText();
    }

    private void IterateText()
    {
        if (_index >= _textQueue.Length) return;
        _label.Text = _textQueue[_index];
        _index += 1;
        _timer.Start();
    }

    private void ClearText()
    {
        _label.Text = "";
        IterateText();
    }

}
