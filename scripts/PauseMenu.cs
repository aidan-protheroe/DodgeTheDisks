using Godot;
using System;

public partial class PauseMenu : Menu
{
	public bool SelectedOption = false;
	public string Option = "";
	
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
			SelectedOption = true;
			Option = "resume";
		}
	}

	public void Label0Selected() {
		SelectedOption = true;
		Option = "resume";
	}

	public void Label1Selected() {
		SelectedOption = true;
		Option = "MainMenu";
	}
}
