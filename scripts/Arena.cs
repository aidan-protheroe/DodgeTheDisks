using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class Arena : Node2D
{
	public Player Player;
	
	public Array<PathFollow2D> Paths;
	public Array<Disk> Disks;
	public List<DiskData> AvailableDisks;
	
	public Timer DiskTimer;
	public Label TimeLabel;
	public ProgressBar PlayerStaminaBar;
	public Node HeartContainer;

	public PackedScene DiskScene;
	public PackedScene HeartScene;
	
	public float TotalTime = 0f; 
	public int Difficulty = 1;
	
	public LoadoutGenerator LG;
	Random rnd;
	
	public bool IncreasedDifficulty = false;
	public bool GeneratedNewDisks = false;
	
	public override void _Ready()
	{
		rnd = new Random();
		
		Player = GetNode<Player>("Player");
		
		HeartScene = GD.Load<PackedScene>("res://scenes/Heart.tscn");
		
		Paths = new Array<PathFollow2D>() {
			GetNode<PathFollow2D>("Path2D/PathFollow2D"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D2"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D3"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D4")
		};

		Disks = new Array<Disk>();

		DiskTimer = GetNode<Timer>("DiskTimer");
		DiskTimer.WaitTime = 2f;
		DiskTimer.Start();
		
		TimeLabel = GetNode<Label>("UI/TimeLabel");
		PlayerStaminaBar = GetNode<ProgressBar>("UI/PlayerStaminaBar");
		HeartContainer = GetNode<Node>("UI/HeartContainer");
		for (int i = 0 ; i < Player.Health ; i++) {
			var h = (Heart) HeartScene.Instantiate();
			h.Position = new Vector2 (80 + (i*70), 30);
			HeartContainer.AddChild(h);
		}

		DiskScene = GD.Load<PackedScene>("res://scenes/Disk.tscn");
		
		AvailableDisks = new List<DiskData>();
		
		//AvailableDisks.Add(DiskDataLoader.Instance.DiskDataDict["SmallRandom"]);
		//AvailableDisks.Add(DiskDataLoader.Instance.DiskDataDict["RegularRandom"]);
		//AvailableDisks.Add(DiskDataLoader.Instance.DiskDataDict["BigRandom"]);
		
		//test
		LG = new LoadoutGenerator();
		AvailableDisks = LG.Generate(1, 3);
	}

	public override void _Process(double delta)
	{
		foreach (PathFollow2D path in Paths) {
			path.Progress++;
		}
		
		TotalTime += (float)delta ;
		TimeLabel.Text = TotalTime.ToString("0.0");
		
		PlayerStaminaBar.Value = (Player.Stamina/Player.MaxStamina) * 100;
		
		if ((int)TotalTime % 60 == 0 && !IncreasedDifficulty) {
			Difficulty++;
			IncreasedDifficulty = true;
			AvailableDisks = LG.Generate(Difficulty, 3);
		} else if (((int)TotalTime % 20) == 0 && !GeneratedNewDisks) {
			AvailableDisks = LG.Generate(Difficulty, 3);
			GeneratedNewDisks = true;
		}
		
		if ((int)TotalTime % 60 == 1) {
			IncreasedDifficulty = false;
		} else if ((int)TotalTime % 20 == 1) {
			GeneratedNewDisks = false;
		}
	}

	private void OnDiskTimerTimeout()
	{
		int SpawnAmount = rnd.Next(Paths.Count);
		var AvailablePaths = new List<int>(){0,1,2,3};
		
		for (int i = 0 ; i < SpawnAmount ; i++) {
			var pathIndex = AvailablePaths[rnd.Next(AvailablePaths.Count)];
			var path = Paths[pathIndex];
			AvailablePaths.Remove(pathIndex);
			Disk disk = (Disk)DiskScene.Instantiate();
			disk.init(path.Position, path.Position.DirectionTo(Player.Position), AvailableDisks[rnd.Next(AvailableDisks.Count)]);
			Disks.Add(disk);
			AddChild(disk);
		}
		
		DiskTimer.WaitTime = rnd.Next(2) + (float)rnd.NextDouble();
		DiskTimer.Start();
	}
}


