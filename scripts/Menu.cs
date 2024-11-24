using Godot;
using System;
using System.Reflection;
using Godot.Collections;

public partial class Menu : Control {
	private AnimatedSprite2D Pointer;
	private Array<Label> Labels;
    private int PointerPosition = 0;

    private string MovementStyle;
    private string BackwardsMovement;
    private string ForwardsMovement;

	private string CurrentPosition = "RestartLabel";
	public override void _Ready()
	{
        Labels = new Array<Label>();
		Pointer = GetNode<AnimatedSprite2D>("Pointer");
		Pointer.Play();

        MovementStyle = "vertical"; //default
        if (MovementStyle == "vertical") {
            BackwardsMovement = "up";
            ForwardsMovement = "down";
        } else if (MovementStyle == "horizontal") {
            BackwardsMovement = "left";
            ForwardsMovement = "right";
        }
	}

	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed(BackwardsMovement) || Input.IsActionJustPressed(ForwardsMovement)) {
            if (Input.IsActionJustPressed(BackwardsMovement)) {
                PointerPosition--;
            } else if (Input.IsActionJustPressed(ForwardsMovement)) {
                PointerPosition++;
            }
            if (PointerPosition < 0) {
                PointerPosition = Labels.Count - 1;
            } else if (PointerPosition > Labels.Count - 1) {
                PointerPosition = Labels.Count - 1;
            }

            if (MovementStyle == "horizontal") {
                Pointer.Position = new Vector2(Labels[PointerPosition].Position.X + (Labels[PointerPosition].Size.X / 2), (Labels[PointerPosition].Position.Y + Labels[PointerPosition].Size.Y + 40));
            } else if (MovementStyle == "vertical") {
                Pointer.Position = new Vector2(Labels[PointerPosition].Position.X - 40, (Labels[PointerPosition].Position.Y + (Labels[PointerPosition].Size.Y / 2)));
            }

        } else if (Input.IsActionJustPressed("enter")) {
            string MethodName = "Label" + PointerPosition + "Selected";
            MethodInfo method = GetType().GetMethod(MethodName);
            method.Invoke(this, null);
        }
	}
}