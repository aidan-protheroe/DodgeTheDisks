using Godot;
using System;
using System.Collections.Generic;

public partial class DiskDataLoader : Node
{
	public static DiskDataLoader Instance {get; private set;}
	
	public Dictionary<string, DiskData> DiskDataDict;
	
	public override void _Ready()
	{
		Instance = this;
		
		var WhiteDiskImage = GD.Load<Texture2D>("res://assets/diskWhite.png");
		
		DiskDataDict = new Dictionary<string, DiskData>() {
			{"Regular", new DiskData() {
				index = 0,
				sprite = WhiteDiskImage,
				scale = new Vector2(1f,1f),
				speed = 5f,
				bounce = false,
				target = "Player"
			}},
			{"SmallRandom", new DiskData() {
				index = 1,
				sprite = WhiteDiskImage,
				scale = new Vector2(.5f,.5f),
				speed = 5f,
				bounce = false,
				target = "Random"
			}},
			{"RegularRandom", new DiskData() {
				index = 2, 
				sprite = WhiteDiskImage,
				scale = new Vector2(1f, 1f),
				speed = 5f,
				bounce = false,
				target = "Random"
			}},
			{"BigRandom", new DiskData() {
				index = 3,
				sprite = WhiteDiskImage,
				scale = new Vector2(2f, 2f),
				speed = 5f,
				bounce = false,
				target = "Random"
			}}
		};
	}
}


public class DiskData {
	public int index {get; set;}
	public Texture2D sprite {get; set;}
	public Vector2 scale {get; set;}
	public float speed {get; set;}
	public bool bounce {get; set;}
	public string target {get; set;}
}

public class LoadoutGenerator {
	
	public Random rnd;
	
	public List<string> TargetOptions;
	
	public List<DiskData> Generate(int difficulty, int numberOfTypes) {
		TargetOptions = new List<string>() {"Random", "Player"};
		rnd = new Random();
		var LoadOut = new List<DiskData>();
		for (int i = 0 ; i < numberOfTypes ; i++) {
			var Points = difficulty;
			var SizePoints = rnd.Next(Points);
			Points -= SizePoints;
			var SpeedPoints = Points;
			
			var size = (rnd.Next(1) + ((float)rnd.NextDouble() + 0.2f) + (SizePoints * .2f));
			LoadOut.Add(new DiskData() {
				index = 0,
				sprite = GD.Load<Texture2D>("res://assets/diskWhite.png"),
				scale = new Vector2(size,size),
				speed = (float)rnd.Next(3 + SpeedPoints) + 3 + (SpeedPoints * .1f),
				bounce = false,
				target = TargetOptions[rnd.Next(TargetOptions.Count)]
			});
		}
		return LoadOut;
	}
}
