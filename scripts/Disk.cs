using Godot;
using System;

public partial class Disk : Area2D
{
	private Vector2 Direction = Vector2.Zero;
	private float Speed;
	public Sprite2D Sprite;
	
	public bool OffScreen;

	public void init(Vector2 position, Vector2 directionToPlayer, DiskData data) {
		Sprite = GetNode<Sprite2D>("Sprite2D");
		
		Position = position;
		
		Sprite.Texture = data.sprite;
		Scale = data.scale;
		Speed = data.speed;
		
		if (data.target == "Random") {
			Direction = Position.DirectionTo(new Vector2(GD.RandRange(600, 1000), GD.RandRange(298, 608))); //remove direct screen size reference and use the built in method to get it
		} else if (data.target == "Player") {
			Direction = directionToPlayer;
		}
	}

	public override void _Process(double delta)
	{
		Sprite.RotationDegrees-=2;
		Position = new Vector2(Position.X + (Direction.X * Speed), Position.Y + (Direction.Y * Speed));
	}
	
	private void OnScreenExited()
	{
		OffScreen = true;
	}
}



