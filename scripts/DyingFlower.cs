using Godot;
using System;

public partial class DyingFlower : AnimatedSprite2D
{

	public bool Shrinking = false;
	public bool DoneShrinking = false;
	
	public override void _Ready()
	{
		Animation = "default";
		Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Shrinking) {
			Scale = new Vector2(Scale.X - 0.025f, Scale.Y - 0.025f);
			if (Scale.X <= 0) {
				QueueFree();
			}
		}
	}
	
	private void OnAnimationFinished()
	{
		Shrinking = true;
	}
}



