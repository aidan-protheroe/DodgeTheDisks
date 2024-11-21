using Godot;
using System;

public partial class Flower : Area2D
{
	public Sprite2D Sprite;
	public bool DoneGrowing = false;
	public Timer LifeTimer;
	public bool Alive = true;
	public bool AnimateDeath = true;
	
	public float LifeTime;
	
	public Random rnd;
	
	public override void _Ready()
	{
		rnd = new Random();
		LifeTimer = GetNode<Timer>("LifeTimer");
		LifeTimer.WaitTime = (float)(rnd.Next(5) + 3);
		LifeTimer.Start();
		
		Sprite = GetNode<Sprite2D>("Sprite2D");
		Scale = new Vector2(0.00f, 0.00f);
		
	}

	public override void _Process(double delta)
	{
		if (!DoneGrowing) {
			Grow();
		}
	}
	
	public void Grow() {
		if (Scale.X < 1f) {
			Scale = new Vector2(Scale.X + 0.025f, Scale.Y + 0.025f);
		} else {
			DoneGrowing = true;
		}
	}
	
	private void OnLifeTimerTimeout()
	{
		Alive = false;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player") {
			var p = (Player) body;
			p.Flowers++;
			Alive = false;
			AnimateDeath = false;
		}
	}
}






