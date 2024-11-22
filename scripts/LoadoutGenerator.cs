using Godot;
using System;
using System.Collections.Generic;

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
