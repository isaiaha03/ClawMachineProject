using Godot;
using System;

public partial class DropZone : Area2D
{
	private bool prizeDetected = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		
		if (body.IsInGroup("prizes"))
		{
			prizeDetected = true;
			ShowWinScreen();
		}
	}
	
	private void ShowWinScreen()
	{
		Label winLabel = GetNode<Label>("/root/Main2D/WinLabel");
		winLabel.Text = "You Won!";
		winLabel.Show();
	}
	
	public bool HasPrize()
	{
		return prizeDetected;
	}
}
