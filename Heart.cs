using Godot;
using System;

public partial class Heart : AnimatedSprite2D
{
	public bool Full = true;
	
	public override void _Ready()
	{
		Animation = "full";
	}

	public void Toggle(int mode) {
		if (mode == 0) {
			Animation = "empty";
			Full = false;
		} else {
			Animation = "full";
			Full = true;
		}
		
	}

}
