using Godot;
using System;

public partial class IdleState : State
{
	public override void Ready() {

	}

	public override void Enter() {
		switch (Player.Facing) {
			case "up":
				Player.Sprite.Animation = "idleUp";
				break;
			case "down":
				Player.Sprite.Animation = "idleDown";
				break;
			case "left":
				Player.Sprite.Animation = "idleSide";
				break;
			case "right":
				Player.Sprite.Animation = "idleSide";
				break;
		}
	}

	public override void Exit() {

	}

	public override void Update(float delta) {
		if (Input.GetVector("left", "right", "up", "down") != Vector2.Zero) {
			fsm.TransitionTo("MovingState");
		}
	}
}
