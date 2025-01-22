using Godot;
using System;

public partial class ClawWire : Node2D
{
	[Export] public NodePath ClawPath;
	private Node2D claw;
	
	public override void _Ready()
	{
		claw = GetNode<Node2D>(ClawPath);
	}
	
	public override void _Draw()
	{
		Vector2 clawPos = claw.GlobalPosition;
		Vector2 wireStart = new Vector2(clawPos.X, 0);
		DrawLine(wireStart, clawPos, new Color(1,1,1), 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
}
