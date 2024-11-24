using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public  float Speed = 300.0f;
	[Export]
	public  float RunSpeed = 600.0f;
	[Export]
	public float MaxStamina = 100f;
	[Export]
	public int MaxHealth = 3;
	[Export]
	public int Flowers = 0;

	public AnimatedSprite2D Sprite;

	public string Facing;
	public int Health;
	public float Stamina;
	

	public override void _Ready() {
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Facing = "down";

		Sprite.Animation = "idleDown";
		Sprite.Play();
		
		Health = MaxHealth;
		Stamina = MaxStamina;
	}
}
