using Godot;
using System;
using System.Collections.Generic;

public partial class ClawMovement : Node2D
{
	[Export] public float Speed = 200f;
	[Export] public float DropSpeed = 250f;
	[Export] public float GrabDepth = 150f;
	[Export] public float ClawCloseDelay = 0.5f;
	[Export] public float ClawOpenDelay = 0.5f;
	
	private Sprite2D _clawSprite;
	private Vector2 _startPosition;
	private State _state = State.Moving;
	
	private enum State { Idle, Moving, Dropping, Closing, Returning, Opening }
	
	public override void _Ready()
	{
		_startPosition = Position;
		_clawSprite = GetNode<Sprite2D>("CharacterBody2D/Sprite2D");
	}
	
	public override void _Process(double delta)
	{
		GD.Print(_state);
		
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
		if (_clawSprite != null)
		{
			_clawSprite.Scale = new Vector2(0.6f, 1.0f);
		}
		
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
		if (_clawSprite != null)
		{
			_clawSprite.Scale = new Vector2(1.0f, 1.0f);
		}
		
		_state = State.Moving;
	}
}
