using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float RunSpeed = 600.0f;
	
	public float Stamina = 100f;
	public float MaxStamina = 100f;
	
	public int Health = 3;
	public int MaxHealth = 3;

	public AnimatedSprite2D Sprite;

	public string Facing;

	public override void _Ready() {
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Facing = "down";

		Sprite.Animation = "idleDown";
		Sprite.Play();
	}
}
