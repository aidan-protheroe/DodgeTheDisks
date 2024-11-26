using Godot;
using System;
using Godot.Collections;

public partial class MainMenu : Menu
{
	public Label PointsLabel;
	public Label HighScoreLabel;
	
	public bool StartGame = false;
	public bool OpenUpgradeMenu = false;
	
	public Random rnd;
	public Timer SpawnTimer;
	public PathFollow2D Path;
	
	public Array<MainMenuItem> Sprites;
	
	public override void _Ready()
	{
		MovementStyle = "vertical";
		base._Ready();
		Labels.Add(GetNode<Label>("StartLabel"));
		Labels.Add(GetNode<Label>("UpgradeLabel"));
		Labels.Add(GetNode<Label>("QuitLabel"));
		
		PointsLabel = GetNode<Label>("PointsLabel");
		HighScoreLabel = GetNode<Label>("HighScoreLabel");
		
		rnd = new Random();
		SpawnTimer = GetNode<Timer>("SpawnTimer");
		Path = GetNode<PathFollow2D>("Path2D/PathFollow2D");
		Sprites = new Array<MainMenuItem>();
		
		SpawnTimer.WaitTime = 2f;
		SpawnTimer.Start();
	}
	
	public override void _Process(double delta) {
		base._Process(delta);
		
		Path.Progress+=rnd.Next(1, 6);
		
		var ToRemove = new Array<MainMenuItem>();
		foreach (MainMenuItem s in Sprites) {
			s.MoveAndRotate();
			if (s.OffScreen) {
				ToRemove.Add(s);
			}
		}
		
		foreach (MainMenuItem s in ToRemove) {
			Sprites.Remove(s);
			s.QueueFree(); 
		}
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

	private void OnSpawnTimerTimeout()
	{
		var s = new MainMenuItem();
		if (rnd.Next(2) == 1) {
			s.init("Disk");
		} else {
			s.init("Flower");
		}
		s.Position = Path.Position;
		Sprites.Add(s);
		AddChild(s);
		SpawnTimer.WaitTime = (float)rnd.Next(1, 3);
	}
}

public partial class MainMenuItem : Sprite2D { //add player aswell? -- also add direction so they aren't all just steraight vertical
	public string Type;
	public int Speed;
	public float Size;
	public Random rnd;
	
	public bool OffScreen = false;
	
	public void init(string type) {
		rnd = new Random();
		Type = type;
		if (Type == "Flower") {
			Texture = GD.Load<Texture2D>("assets/flower.png");
			Size = 2f;
			Speed = 3;
		} else if (Type == "Disk") {
			Texture = GD.Load<Texture2D>("assets/diskWhite.png");
			Size = (float)(rnd.NextDouble() + 0.2f);
			Speed = rnd.Next(3, 6);
			GD.Print(Texture);
		}
		Scale = new Vector2(Size, Size);
	}
	
	public void MoveAndRotate() {
			var pos = Position;
			pos.Y-=Speed;
			Position = pos;
			RotationDegrees--;
			
			if (pos.Y <= -100) {
				OffScreen = true;
			}
	}
	
}
