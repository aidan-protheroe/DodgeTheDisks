using Godot;
using System;

public partial class RestartMenu : Control
{
	private Label RestartLabel;
	private Label MainMenuLabel;
	private AnimatedSprite2D Pointer;
	
	public bool NewGame = false;

	private string CurrentPosition = "RestartLabel";
	public override void _Ready()
	{
		RestartLabel = GetNode<Label>("RestartLabel");
		MainMenuLabel = GetNode<Label>("MainMenuLabel");
		Pointer = GetNode<AnimatedSprite2D>("Pointer");

		Pointer.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("up") || Input.IsActionJustPressed("down")) {
			if (CurrentPosition == "RestartLabel") {
				CurrentPosition = "MainMenuLabel";
				Pointer.Position = new Vector2(MainMenuLabel.Position.X - 30, MainMenuLabel.Position.Y + 30);
			} else {
				CurrentPosition = "RestartLabel";
				Pointer.Position = new Vector2(RestartLabel.Position.X - 30, RestartLabel.Position.Y + 30);
			}
		}

		if (Input.IsActionJustPressed("enter")) {
			if (CurrentPosition == "RestartLabel") {
				NewGame = true;
			} else if (CurrentPosition == "MainMenuLabel") {
				GetTree().Quit();
			}
		} 
	}
}
