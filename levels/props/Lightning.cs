using Godot;

public partial class Lightning : AnimatedSprite2D
{
	[Export] public float ThunderMinDelay = 4.0f;
	[Export] public float ThunderMaxDelay = 12.0f;
	[Export] public int SoundDelayMs = 200;
	[Export] public float Margin = 200.0f;
	
	private AudioStreamPlayer _audioStreamPlayer;
	private AnimationPlayer _animationPlayer;
	private Timer _timer;
	
	public override void _Ready()
	{
		_audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_timer = GetNode<Timer>("Timer");

		_timer.Timeout += _thor;
		_startLightningTimer();
	}
	
	private void _startLightningTimer()
	{
		var nextStrike = (float)GD.RandRange(ThunderMinDelay, ThunderMaxDelay);
		_timer.WaitTime = nextStrike;
		_timer.Start();
	}

	private async void _thor()
	{
		var visibleRect = GetViewport().GetVisibleRect();
		var halfWidth = visibleRect.Size.X / 2.0f;
		var randomX = (float)GD.RandRange(-halfWidth + Margin, halfWidth - Margin);	
		Position = new Vector2(randomX, Position.Y);
		FlipH = randomX < 0;
		_animationPlayer.Play("LightningStrike");
		
		if (SoundDelayMs > 0)
			await ToSignal(GetTree().CreateTimer(SoundDelayMs / 1000.0f), SceneTreeTimer.SignalName.Timeout);
		
		_audioStreamPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
		_audioStreamPlayer.Play();
		
		_startLightningTimer();
	}
}
