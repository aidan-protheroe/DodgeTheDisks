using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class Arena : Node2D
{
	//Objects
	public Random rnd;
	public LoadoutGenerator LG;
	public ItemLoader IL;
	//Nodes
	public Player Player;
	public Timer DiskTimer;
	public Label TimeLabel;
	public ProgressBar PlayerStaminaBar;
	public Node HeartContainer;
	public GridContainer HeartGrid;
	public Timer FlowerTimer;
	public Label FlowerLabel;
	public CanvasLayer UI;
	//scenes
	public PackedScene DiskScene;
	public PackedScene HeartScene;
	public PackedScene BloodSplatterScene;
	public PackedScene FlowerScene;
	public PackedScene DyingFlowerScene;
	public PackedScene ShopScene;
	//arrays
	public Array<PathFollow2D> Paths;
	public Array<Disk> Disks;
	public List<DiskData> AvailableDisks;
	public Array<Heart> Hearts;
	public Array<Flower> Flowers;
	public List<Item> AvailableItems;
	//data
	public float TotalTime = 0f; 
	public int Difficulty = 0;
	public bool IncreasedDifficulty = false;
	public bool OpenedShop = false;
	public bool GeneratedNewDisks = false;
	public int FlowerSpawnRate = 10;
	public int ShopNumber = 1;
	
	public override void _Ready()
	{
		
		rnd = new Random();
		LG = new LoadoutGenerator();
		IL = new ItemLoader();
		
		LoadNodes();
		LoadScenes();
		LoadArrays();
		

		
		FlowerTimer.WaitTime = (float)(rnd.Next(10) + 3);
		FlowerTimer.Start();
		
		FlowerLabel.Text = "x" + Player.Flowers;
		
		DiskTimer.WaitTime = 2f;
		DiskTimer.Start();
		
		for (int i = 0 ; i < Player.Health ; i++) {
			var h = (Heart) HeartScene.Instantiate();
			Hearts.Add(h);
			HeartGrid.AddChild(h);
		}
		
		AvailableDisks = LG.Generate(0, 3);
	}
	
	private void LoadNodes() {
		Player = GetNode<Player>("Player");
		//Timers
		FlowerTimer = GetNode<Timer>("FlowerTimer");
		DiskTimer = GetNode<Timer>("DiskTimer");
		//UI
		UI = GetNode<CanvasLayer>("UI");
		TimeLabel = GetNode<Label>("UI/TimeLabel");
		PlayerStaminaBar = GetNode<ProgressBar>("UI/PlayerStaminaBar");
		HeartContainer = GetNode<Node>("UI/HeartContainer");
		HeartGrid = GetNode<GridContainer>("UI/HeartGrid");
		FlowerLabel = GetNode<Label>("UI/FlowerLabel");
	}
	
	private void LoadScenes() {
		HeartScene = GD.Load<PackedScene>("res://scenes/Heart.tscn");
		BloodSplatterScene = GD.Load<PackedScene>("res://scenes/BloodSplatter.tscn");
		FlowerScene = GD.Load<PackedScene>("res://scenes/Flower.tscn");
		DyingFlowerScene = GD.Load<PackedScene>("res://scenes/DyingFlower.tscn");
		DiskScene = GD.Load<PackedScene>("res://scenes/Disk.tscn");
		ShopScene = GD.Load<PackedScene>("res://scenes/Shop.tscn");
	}
	
	private void LoadArrays() {
		Hearts = new Array<Heart>();
		Flowers = new Array<Flower>();
		Disks = new Array<Disk>();
		
		Paths = new Array<PathFollow2D>() {
			GetNode<PathFollow2D>("Path2D/PathFollow2D"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D2"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D3"),
			GetNode<PathFollow2D>("Path2D/PathFollow2D4")
		};
		
		AvailableDisks = new List<DiskData>();
		AvailableItems = IL.LevelOneItems;
	}

	public override void _Process(double delta) //split into several methods
	{
		TotalTime += (float)delta;
		
		foreach (PathFollow2D path in Paths) {
			path.Progress++;
		}
		
		TimeLabel.Text = TotalTime.ToString("0.0");
		PlayerStaminaBar.Value = (Player.Stamina/Player.MaxStamina) * 100;
		
		if (TotalTime > 1) {
			CheckTimeFlags();
		}
		
		
		if (Disks.Count > 0) {
			HandleDisks();
		}
		
		if (Flowers.Count > 0) {
			HandleFlowers();
		}
		
	}
	
	private void CheckTimeFlags() {
		if ((int)TotalTime % 10 == 0 && !OpenedShop) {
			OpenedShop = true; //add open shop method
			var s = (Shop)ShopScene.Instantiate();
			UI.AddChild(s);
			s.init(this);
			GetTree().Paused = true;
			ShopNumber++;
			IL = new ItemLoader();
			AvailableItems = IL.LevelOneItems;
			if (ShopNumber >= 2) {
				foreach (var i in IL.LevelTwoItems) {
					AvailableItems.Add(i);
				}
			} 
			if (ShopNumber >= 3) {
				foreach (var i in IL.LevelThreeItems) {
					AvailableItems.Add(i);
				}
			}
			
			 //swithc to include leve 2 and 3 for 2nd and 3rd shops
			
		}
		if ((int)TotalTime % 60 == 0 && !IncreasedDifficulty) {
			Difficulty++;
			IncreasedDifficulty = true;
			AvailableDisks = LG.Generate(Difficulty, 3);
		} else if (((int)TotalTime % 20) == 0 && !GeneratedNewDisks) {
			AvailableDisks = LG.Generate(Difficulty, 3);
			GeneratedNewDisks = true;
		}
		
		if ((int)TotalTime % 10 == 1) {
			OpenedShop = false;
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
	
	private void PlayerHit() { //add death conditions
		Player.Health--;
		Hearts[Player.Health].Toggle(0);
		
		var bs = (BloodSplatter) BloodSplatterScene.Instantiate();
		AddChild(bs);
		bs.Position = Player.Position;
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
		
		FlowerTimer.WaitTime = (float)(rnd.Next(FlowerSpawnRate) + 3);
		FlowerTimer.Start();
	}
	
	
	//UPGRADE METHODS:
	public void HealPlayer(float amount) { //add amount = 0 heal all heartrs
		for (int i = 0 ; i < amount; i++) {
			if (Player.Health < Player.MaxHealth) {
				Hearts[Player.Health].Toggle(1);
				Player.Health++;
			}
		}
	}
	
	public void RefillPlayerStamina(float percent) {
		Player.Stamina += Player.MaxStamina * percent;
		if (Player.Stamina > Player.MaxStamina) {
			Player.Stamina = Player.MaxStamina;
		}
		PlayerStaminaBar.Value = (Player.Stamina/Player.MaxStamina) * 100;
	} 
	
	public void PlusPlayerHeart(int amount) {
		for (int i = 0 ; i < amount ; i++) {
			Player.MaxHealth++;
			Player.Health++;
			var h = (Heart) HeartScene.Instantiate();
			Hearts.Add(h);
			HeartGrid.AddChild(h);
		}
		var sizeY = Math.Ceiling((double)Hearts.Count / 3);
		HeartGrid.Size = new Vector2(HeartGrid.Size.X, (float)((55 * sizeY)));
	}
	
	public void PlusPlayerStamina(float amount) {
		Player.MaxStamina *= amount;
		Player.Stamina *= amount;
	}
	
	public void PlusPlayerSpeed(float amount) {
		Player.Speed *= amount;
		Player.RunSpeed *= amount;
	}
	
	public void IncreaseFlowerSpawnRate(int amount) {
		FlowerSpawnRate -= amount;
	}
}
