using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class Arena : Node2D
{
	public Player Player;
	public LoadoutGenerator LG;
	public Random rnd;
	
	public Timer DiskTimer;
	public Label TimeLabel;
	public ProgressBar PlayerStaminaBar;
	public Node HeartContainer;
	public GridContainer HeartGrid;
	public Timer FlowerTimer;
	public Label FlowerLabel;

	public PackedScene DiskScene;
	public PackedScene HeartScene;
	public PackedScene BloodSplatterScene;
	public PackedScene FlowerScene;
	public PackedScene DyingFlowerScene;
	
	public Array<PathFollow2D> Paths;
	public Array<Disk> Disks;
	public List<DiskData> AvailableDisks;
	public Array<Heart> Hearts;
	public Array<Flower> Flowers;
	
	public float TotalTime = 0f; 
	public int Difficulty = 0;
	
	public bool IncreasedDifficulty = false;
	public bool GeneratedNewDisks = false;
	
	public override void _Ready()
	{
		rnd = new Random();
		
		Player = GetNode<Player>("Player");
		
		FlowerTimer = GetNode<Timer>("FlowerTimer");
		FlowerTimer.WaitTime = (float)(rnd.Next(10) + 3);
		FlowerTimer.Start();
		
		HeartScene = GD.Load<PackedScene>("res://scenes/Heart.tscn");
		BloodSplatterScene = GD.Load<PackedScene>("res://scenes/BloodSplatter.tscn");
		FlowerScene = GD.Load<PackedScene>("res://scenes/Flower.tscn");
		DyingFlowerScene = GD.Load<PackedScene>("res://scenes/DyingFlower.tscn");
		
		Hearts = new Array<Heart>();
		Flowers = new Array<Flower>();
		
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
		HeartGrid = GetNode<GridContainer>("UI/HeartGrid");
		
		FlowerLabel = GetNode<Label>("UI/FlowerLabel");
		
		for (int i = 0 ; i < Player.Health ; i++) {
			var h = (Heart) HeartScene.Instantiate();
			Hearts.Add(h);
			HeartGrid.AddChild(h);
		}

		DiskScene = GD.Load<PackedScene>("res://scenes/Disk.tscn");
		
		AvailableDisks = new List<DiskData>();
		
		LG = new LoadoutGenerator();
		AvailableDisks = LG.Generate(0, 3);
	}

	public override void _Process(double delta) //split into several methods
	{
		TotalTime += (float)delta;
		
		foreach (PathFollow2D path in Paths) {
			path.Progress++;
		}
		
		TimeLabel.Text = TotalTime.ToString("0.0");
		PlayerStaminaBar.Value = (Player.Stamina/Player.MaxStamina) * 100;
		
		CheckTimeFlags();
		
		if (Disks.Count > 0) {
			HandleDisks();
		}
		
		if (Flowers.Count > 0) {
			HandleFlowers();
		}
	}
	
	private void CheckTimeFlags() {
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
	
	private void HandleDisks() {
		var DisksToRemove = new Array<Disk>();
		foreach (Disk d in Disks) {
			if (d.OffScreen) {
				DisksToRemove.Add(d);
			} else if (d.HasOverlappingBodies()) {
				foreach (var b in d.GetOverlappingBodies()) {
					if (b.Name == "Player") {
						PlayerHit();
						RemoveAllDisks();
					}
				}
			}
		}
			
		foreach (Disk d in DisksToRemove) {
			Disks.Remove(d);
			d.QueueFree();
		}
	}
	
	private void PlayerHit() {
		Player.Health--;
		var bs = (BloodSplatter) BloodSplatterScene.Instantiate();
		AddChild(bs);
		bs.Position = Player.Position;
		
		for (int i = Hearts.Count - 1; i >= 0 ; i--) { //just add a method that takes player healrh and does all this
			if (Hearts[i].Full) {
				Hearts[i].Toggle(0);
				break;
			}
		}
	}
	
	private void RemoveAllDisks() {
		foreach (Disk disk in Disks) {
			disk.QueueFree();
		}
		Disks = new Array<Disk>();
	}
	
	private void HandleFlowers() {
		var FlowersToRemove = new Array<Flower>();
		foreach (Flower f in Flowers) {
			if (f.Alive == false) {
				if (f.AnimateDeath) {  //player did not collect
					var df = (DyingFlower)DyingFlowerScene.Instantiate();
					df.Position = f.Position;
					AddChild(df);
				} else { //player did collect
					FlowerLabel.Text = "x" + Player.Flowers; //move this elsewhere, make flower collision work like disk 
				}
				FlowersToRemove.Add(f);
				f.QueueFree();
			}
		}
		
		foreach (Flower f in FlowersToRemove) {
			Flowers.Remove(f);
		}
	}

	private void OnDiskTimerTimeout()
	{
		//SPAWN DISKS
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
	
	private void OnFlowerTimerTimeout()
	{
		//SPAWN A FLOWER
		var flower = (Flower)FlowerScene.Instantiate();
		flower.Position = new Vector2(rnd.Next(704) + 448, rnd.Next(640) + 128);
		Flowers.Add(flower);
		AddChild(flower);
		
		FlowerTimer.WaitTime = (float)(rnd.Next(10) + 3);
		FlowerTimer.Start();
	}
}





