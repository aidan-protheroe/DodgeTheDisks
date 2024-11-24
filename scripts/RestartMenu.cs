using Godot;
using System;

public partial class RestartMenu : Menu
{
	public bool NewGame = false;

	public override void _Ready()
	{
		MovementStyle = "vertical";
		base._Ready();
		Labels.Add(GetNode<Label>("RestartLabel"));
		Labels.Add(GetNode<Label>("MainMenuLabel"));
	}

	public void Label0Selected() {
		NewGame = true;
	}

	public void Label1Selected() {
		GetTree().Quit();
	}
}
