using Godot;
using System;

public partial class Player : CharacterBody2D, IKillable
{
    private AnimationTree _animationTree;
    private AnimationPlayer _animationPlayer;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Sprite2D _sprite2D;
    private RayCast2D _rayCast2D;
    private Vector2 _currentRespawnPosition = new(0, 0);
    private bool _isDead = false;

    [Export] public float Speed = 20.0f;
    [Export] public float MaxSpeed = 300.0f;
    [Export] public float BrakingSpeed = 10.0f;
    [Export] public float JumpVelocity = -500.0f;
    [Export] public float ExtraDeathTime = 0.5f;
    [Export] public PlayerDialogue Dialogue { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _rayCast2D = GetNode<RayCast2D>("RayCast2D");
        _currentRespawnPosition = GlobalPosition;

        GameEvents.Instance.CheckpointActivated += OnCheckPointActivated;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        var direction = Input.GetAxis("move_left", "move_right");
        var isJumping = false;

        if (!IsOnFloor())
            velocity += GetGravity() * (float)delta;

        if (_isDead)
        {
            Velocity = velocity;
            MoveAndSlide();
            return;
        }

        if (Input.IsActionJustPressed("move_up") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            isJumping = true;
        }

        if (direction == 0)
            velocity.X = Mathf.MoveToward(Velocity.X, 0, BrakingSpeed);
        else
        {
            if (Mathf.Abs(velocity.X + Speed * direction) > MaxSpeed)
                velocity.X = direction * MaxSpeed;
            else
                velocity.X += direction * Speed;
        }

        Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation(direction, isJumping);
    }

    private void UpdateAnimation(float direction, bool isJumping)
    {
        var velocity = Velocity;

        if (direction != 0)
            _sprite2D.FlipH = direction < 0;

        if (IsOnFloor())
        {
            _stateMachine.Travel((direction == 0) ? "idle" : "run");
        }
        else
        {
            if (isJumping)
                _stateMachine.Travel("jump");
            else if (_rayCast2D.IsColliding())
                _stateMachine.Travel("land");
            else if (velocity.Y >= 0)
                _stateMachine.Travel("fall");
        }
    }

    public async void Die()
    {
        if (_isDead) return;

        _isDead = true;
        Velocity = Vector2.Zero;
        _stateMachine.Start("die");
        var wait = _animationPlayer.GetAnimation("die").Length + ExtraDeathTime;
        await ToSignal(GetTree().CreateTimer(wait), "timeout");
        Respawn();
    }

    public void Respawn()
    {
        _isDead = false;
        _stateMachine.Travel("idle");
        Teleport(_currentRespawnPosition);
    }

    private void Teleport(Vector2 position)
    {
        Velocity = Vector2.Zero;
        GlobalPosition = position;
    }

    private void OnCheckPointActivated(Vector2 position)
    {
        _currentRespawnPosition = position;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && GameEvents.Instance != null)
        {
            GameEvents.Instance.CheckpointActivated -= OnCheckPointActivated;
        }
        base.Dispose(disposing);
    }
}
