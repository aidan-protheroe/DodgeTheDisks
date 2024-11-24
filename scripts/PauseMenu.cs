using Godot;
using System;

public partial class PauseMenu : Menu
{
	public bool SelectedOption = false;
	
	public bool Active = true;
	public bool GoToMainMenu = false;
	
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
		SelectedOption = true;
		Active = false; //use a string Option instead
	}

	public void Label1Selected() {
		SelectedOption = true;
		GoToMainMenu = true; //use a string Option instead
	}
}
