using Godot;
using System;

public partial class Heart : TextureRect
{
	public bool Full = true;
	
	public Texture2D FullTexture;
	public Texture2D EmptyTexture;
	
	public override void _Ready()
	{
		FullTexture = GD.Load<Texture2D>("res://assets/HeartFull.png");
		EmptyTexture = GD.Load<Texture2D>("res://assets/HeartEmpty.png");
		Texture = FullTexture;
	}

	public void Toggle(int mode) {
		if (mode == 0) {
			Texture = EmptyTexture;
			Full = false;
		} else {
			Texture = FullTexture;
			Full = true;
		}
		
	}

}
