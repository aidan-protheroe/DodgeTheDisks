using Godot;
using System;

public partial class MovingState : State
{
	private bool Running = false;
	private float Speed;
	
	public override void Ready() {
		Speed = Player.Speed;
	}

	public override void Enter() {

	}

	public override void Exit() {

	}

	public override void Update(float delta) {
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		HandleMovement(direction);
		HandleAnimation(direction);

		if (Player.Velocity == Vector2.Zero) {
			fsm.TransitionTo("IdleState");
		} 
		
		if (Input.IsActionPressed("run") && Player.Stamina > 0) {
			Speed = Player.RunSpeed;
			Player.Stamina -= .25f;
			Running = true;
		} else {
			Speed = Player.Speed;
			Running = false;
		}
	}

	public void HandleMovement(Vector2 direction) {
		Vector2 velocity = Player.Velocity;

		direction = direction.Normalized();

		if (direction != Vector2.Zero) {
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		} else {
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(velocity.Y, 0, Speed);
		}

		Player.Velocity = velocity;
		Player.MoveAndSlide();
	}

	public void HandleAnimation(Vector2 direction) {
		string action = "";
		if (Running) {
			action = "run";
		} else {
			action = "move";
		}
		
		if (direction.Y > 0) {
			Player.Sprite.Animation = action + "Down";
			Player.Facing = "down";
		} else if (direction.Y < 0) {
			Player.Sprite.Animation = action + "Up";
			Player.Facing = "up";
		} else if (direction.X < 0) {
			Player.Sprite.Animation = action + "Side";
			Player.Facing = "left";
		} else if (direction.X > 0) {
			Player.Sprite.Animation = action + "Side";
			Player.Facing = "right";
		}

		if (Player.Facing == "left") {
			Player.Sprite.FlipH = true;
		} else if (Player.Facing == "right") {
			Player.Sprite.FlipH = false;
		}
	}
}
