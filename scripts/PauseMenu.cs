using Godot;
using System;

public partial class PauseMenu : Control
{
	private Label ContinueLabel;
	private Label ExitLabel;
	private AnimatedSprite2D Pointer;

	public bool Active = true;
	private string CurrentPosition = "ContinueLabel";
	public override void _Ready()
	{
		ContinueLabel = GetNode<Label>("ContinueLabel");
		ExitLabel = GetNode<Label>("ExitLabel");
		Pointer = GetNode<AnimatedSprite2D>("Pointer");

		Pointer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("up") || Input.IsActionJustPressed("down")) {
			if (CurrentPosition == "ContinueLabel") {
				CurrentPosition = "ExitLabel";
				Pointer.Position = new Vector2(ExitLabel.Position.X - 30, ExitLabel.Position.Y + 30);
			} else {
				CurrentPosition = "ContinueLabel";
				Pointer.Position = new Vector2(ContinueLabel.Position.X - 30, ContinueLabel.Position.Y + 30);
			}
		}

		if (Input.IsActionJustPressed("enter")) {
			if (CurrentPosition == "ContinueLabel") {
				Active = false;
			} else if (CurrentPosition == "ExitLabel") {
				GetTree().Quit();
			}
		} else if (Input.IsActionJustPressed("escape")) {
			Active = false;
		}
	}
}
