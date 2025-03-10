using Godot;
using System;

public partial class Buttons : Control
{
	private TouchScreenButton _left;
	private TouchScreenButton _right;
	private TouchScreenButton _down;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_left = GetNode<TouchScreenButton>("Left");
		_right = GetNode<TouchScreenButton>("Right");
		_down = GetNode<TouchScreenButton>("Down");
	}

	private void OnLeftPressed()
	{
		SetAlpha(_left, 0.5f);
	}
	private void OnLeftReleased()
	{
		SetAlpha(_left, 1.0f);
	}
	private void OnRightPressed()
	{
		SetAlpha(_right, 0.5f);
	}
	private void OnRightReleased()
	{
		SetAlpha(_right, 1.0f);
	}
	private void OnDownPressed()
	{
		SetAlpha(_down, 0.5f);
	}
	private void OnDownReleased()
	{
		SetAlpha(_down, 1.0f);
	}
	
	private void SetAlpha(Node2D node, float alpha)
	{
		var modulate = node.Modulate;
		modulate.A = alpha;
		node.Modulate = modulate;
	}
}
