using Godot;
using System;

public partial class MainMenu : Menu
{
	public Label PointsLabel;
	public Label HighScoreLabel;
	
	public bool StartGame = false;
	public bool OpenUpgradeMenu = false;
	
	public override void _Ready()
	{
		MovementStyle = "vertical";
		base._Ready();
		Labels.Add(GetNode<Label>("StartLabel"));
		Labels.Add(GetNode<Label>("UpgradeLabel"));
		Labels.Add(GetNode<Label>("QuitLabel"));
	}
	
	public override void _Process(double delta) {
		base._Process(delta);
	}

	public void Label0Selected() {
		StartGame = true;
	}

	public void Label1Selected() {
		OpenUpgradeMenu = true;
	}
	
	public void Label2Selected() {
		GetTree().Quit();
	}
}
