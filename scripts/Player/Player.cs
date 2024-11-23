using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public  float Speed = 300.0f;
	public  float RunSpeed = 600.0f;
	
	public float Stamina = 100f;
	public float MaxStamina = 100f;
	
	public int Health = 3;
	public int MaxHealth = 3;

	public AnimatedSprite2D Sprite;

	public string Facing;
	
	public int Flowers = 100;

	public override void _Ready() {
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Facing = "down";

		Sprite.Animation = "idleDown";
		Sprite.Play();
	}
	
	public void Hit() { //not using rn
		Health--;
		//bloodsplatter -- actually prob has to be done by arena class
		//Make invincible???
		//add death check
	}
}
