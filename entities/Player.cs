using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Sprite2D _sprite2D;
    private RayCast2D _rayCast2D;

    private const float Speed = 300.0f;
    private const float JumpVelocity = -500.0f;

    public override void _Ready()
    {
        base._Ready();
        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _rayCast2D = GetNode<RayCast2D>("RayCast2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        float direction = Input.GetAxis("move_left", "move_right");
        var isJumping = false;

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("move_up") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            isJumping = true;
        }

        if (direction == 0)
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        else
        {
            velocity.X = direction * Speed;
        }

        Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation(direction, isJumping);
    }

    private void UpdateAnimation(float direction, bool isJumping)
    {
        Vector2 velocity = Velocity;

        if (direction != 0)
        {
            _sprite2D.FlipH = direction < 0;
        }

        if (IsOnFloor())
        {
            if (direction == 0)
            {
                _stateMachine.Travel("idle");
            }
            else
            {
                _stateMachine.Travel("run");
            }
        }
        else
        {
            if (isJumping)
            {
                _stateMachine.Travel("jump");
            }
            else if (_rayCast2D.IsColliding())
            {
                _stateMachine.Travel("land");
            }
            else if (velocity.Y >= 0)
            {
                _stateMachine.Travel("fall");
            }
        }
    }
}