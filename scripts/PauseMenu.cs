using Godot;
using System;

public partial class PauseMenu : Menu
{
	public bool Active = true;
	public override void _Ready()
	{
		MovementStyle = "vertical";
		base._Ready();
		Labels.Add(GetNode<Label>("ContinueLabel"));
		Labels.Add(GetNode<Label>("ExitLabel"));
	}
	
	public override void _Process(double delta) {
		base._Process(delta);
		if (Input.IsActionJustPressed("escape")) {
			Active = false;
		}
	}

	public void Label0Selected() {
		Active = false;
	}

	public void Label1Selected() {
		GetTree().Quit();
	}
}
