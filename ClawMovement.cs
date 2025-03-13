using Godot;
using System;
using System.Collections.Generic;

public partial class ClawMovement : Node2D
{
	[Export] public float Speed = 200f;
	[Export] public float DropSpeed = 300f;
	[Export] public float GrabDepth = 475f;
	[Export] public float ClawCloseDelay = 0.1f;
	[Export] public float ClawOpenDelay = 0.1f;
	
	private CharacterBody2D _clawLeftArmBottom;
	private CharacterBody2D _clawRightArmBottom;
	private Vector2 _startPosition;
	private State _state = State.Moving;
	private Tween _tween;
	
	private enum State { Idle, Moving, Dropping, Closing, Returning, Opening }
	
	public override void _Ready()
	{
		_startPosition = Position;
		_clawLeftArmBottom = GetNode<CharacterBody2D>("ClawBody/LeftArmBottom");
		_clawRightArmBottom = GetNode<CharacterBody2D>("ClawBody/RightArmBottom");
		
	}
	
	public override void _Process(double delta)
	{
		switch (_state)
		{
			case State.Moving:
				HandleInput();
				if (Input.IsActionJustPressed("down")) StartDropping();
				break;
			
			case State.Dropping:
				DropClaw((float)delta);
				break;
			
			case State.Closing:
				break;
				
			case State.Returning:
				ReturnToStart((float)delta);
				break;
				
			case State.Opening:
				break;
		}
	}
	
	private void HandleInput()
	{
		Vector2 velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("left")) velocity.X = -1;
		if (Input.IsActionPressed("right")) velocity.X = 1;
		
		Position += velocity.Normalized() * Speed * (float)GetProcessDeltaTime();
	}
	
	private void StartDropping()
	{
		_state = State.Dropping;
	}
	
	private void DropClaw(float delta)
	{
		Position += Vector2.Down * DropSpeed * delta;
		
		if (Position.Y >= _startPosition.Y + GrabDepth)
		{
			_state = State.Closing;
			GetTree().CreateTimer(ClawCloseDelay).Timeout += CloseClaw;
		}
	}
	
	private void CloseClaw()
	{
		if (_tween == null || !_tween.IsValid())
		{
			_tween = CreateTween();
		}
		else
		{
			_tween.Kill();
			_tween = CreateTween();
		}
		
		_tween.TweenProperty(_clawLeftArmBottom, "position:x", _clawLeftArmBottom.Position.X + 10, 0.025f);
		_tween.TweenProperty(_clawRightArmBottom, "position:x", _clawRightArmBottom.Position.X - 10, 0.025f);
		
		_state = State.Returning;
	}
	
	private void ReturnToStart(float delta)
	{
		Position = Position.MoveToward(_startPosition, Speed * delta);
		
		if (Position.DistanceTo(_startPosition) < 5f )
		{
			_state = State.Opening;
			GetTree().CreateTimer(ClawOpenDelay).Timeout += OpenClaw;
		}
	}
	
	private void OpenClaw()
	{
		if (_tween == null || !_tween.IsValid())
		{
			_tween = CreateTween();
		}
		else
		{
			_tween.Kill();
			_tween = CreateTween();
		}
		
		_tween.TweenProperty(_clawLeftArmBottom, "position:x", _clawLeftArmBottom.Position.X - 10, 0.2f);
		_tween.TweenProperty(_clawRightArmBottom, "position:x", _clawRightArmBottom.Position.X + 10, 0.2f);
		
		_state = State.Moving;
	}
}
