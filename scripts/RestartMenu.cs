using Godot;
using System;

public partial class RestartMenu : Menu
{
	public bool SelectedOption = false;
	
	public string Option;

	public override void _Ready()
	{
		MovementStyle = "vertical";
		base._Ready();
		Labels.Add(GetNode<Label>("RestartLabel"));
		Labels.Add(GetNode<Label>("MainMenuLabel"));
	}

	public void Label0Selected() {
		SelectedOption = true;
		Option = "NewGame";
	}

	public void Label1Selected() {
		SelectedOption = true;
		Option = "MainMenu";
	}
}
