using Godot;
using System;

public partial class ShopCard : Control
{
	public Label PriceLabel;
	public Sprite2D ItemSprite;
	
	public Item Item;
	
	public int Speed = 8;
	public string VDirection = "up";
	public string HDirection = "left";
	public bool BackToCenter = false;
	
	public int Steps = 0;
	public bool FinishedPurchaseAnimation = true;
	public bool FinishedNoPurchaseAnimation = true;
	
	public override void _Ready()
	{
		PriceLabel = GetNode<Label>("PriceLabel");
		ItemSprite = GetNode<Sprite2D>("ItemSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!FinishedPurchaseAnimation) {
			AnimatePurchase();
		} else if (!FinishedNoPurchaseAnimation) {
			AnimateNoPurchase();
		}
	}
	
	public void AnimatePurchase() {
		if (VDirection == "up") {
			Position = new Vector2(Position.X, Position.Y - Speed);
			Steps++;
			if (Steps >= 10) {
				VDirection = "down";
				Steps = 0;
			}
		} else if (VDirection == "down") {
			Position = new Vector2(Position.X, Position.Y + Speed);
			Steps++;
			if (Steps >= 10) {
				FinishedPurchaseAnimation = true;
				Steps = 0;
				VDirection = "up";
			}
		}
		
	}
	
	private void AnimateNoPurchase() {
		if (HDirection == "left") {
			RotationDegrees -= 3;
			Steps++;
			if (Steps >= 5) {
				if (BackToCenter) {
					FinishedNoPurchaseAnimation = true;
					Steps = 0;
					BackToCenter = false;
					
				} else {
					HDirection = "right";
					Steps = 0;
				}

			}
		} else if (HDirection == "right") {
			RotationDegrees += 3;
			Steps++;
			if (Steps >= 10) {
				HDirection = "left";
				Steps = 0;
				BackToCenter = true;
			}
		}
	}
}
