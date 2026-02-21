using Godot;

public partial class Lightning : AnimatedSprite2D
{
    [Export] public float ThunderMinDelay = 4.0f;
    [Export] public float ThunderMaxDelay = 12.0f;
    [Export] public float SoundDelay = 0.2f;
    [Export] public float Margin = 200.0f;

    private AudioStreamPlayer _audioStreamPlayer;
    private AnimationPlayer _animationPlayer;
    private Timer _timer;

    public override void _Ready()
    {
        _audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _timer = GetNode<Timer>("Timer");

        _timer.Timeout += Thor;
        StartLightningTimer();
    }

    private void StartLightningTimer()
    {
        var nextStrike = (float)GD.RandRange(ThunderMinDelay, ThunderMaxDelay);
        _timer.WaitTime = nextStrike;
        _timer.Start();
    }

    private async void Thor()
    {
        var visibleRect = GetViewport().GetVisibleRect();
        var halfWidth = visibleRect.Size.X / 2.0f;
        var effectiveMargin = Mathf.Min(Margin, halfWidth);
        var randomX = (float)GD.RandRange(-halfWidth + effectiveMargin, halfWidth - effectiveMargin);
        Position = new Vector2(randomX, Position.Y);
        FlipH = randomX < 0;
        _animationPlayer.Play("LightningStrike");

        if (SoundDelay > 0)
            await ToSignal(GetTree().CreateTimer(SoundDelay), SceneTreeTimer.SignalName.Timeout);

        _audioStreamPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
        _audioStreamPlayer.Play();

        StartLightningTimer();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _timer.Timeout -= Thor;
    }
}
