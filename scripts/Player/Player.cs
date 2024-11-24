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
	
	public bool HurtAnimation = false;
	public int HurtAnimationLoops = 3;
	public bool LessOpaque = true;
	

	public override void _Ready() {
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Facing = "down";

		Sprite.Animation = "idleDown";
		Sprite.Play();
		
		Health = MaxHealth;
		Stamina = MaxStamina;
		
		//HurtAnimation = true;
	}
	
	public override void _Process(double delta) {
		if (HurtAnimation && HurtAnimationLoops > 0) {
			if (LessOpaque) {
				var color = Sprite.SelfModulate;
				color.A -= 0.08f;
				Sprite.SelfModulate = color;
				if (Sprite.SelfModulate.A < 0.35f) {
					LessOpaque = false;
				}
			} else if (!LessOpaque) {
				var color = Sprite.SelfModulate;
				color.A += 0.08f;
				Sprite.SelfModulate = color;
				if (Sprite.SelfModulate.A == 1) {
					HurtAnimationLoops--;
					LessOpaque = true;
				}
			}
		} else if (HurtAnimationLoops <= 0) {
			HurtAnimationLoops = 3;
			HurtAnimation = false;
			LessOpaque = true;
		}
	}
}
