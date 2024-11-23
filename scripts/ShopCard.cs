using Godot;
using System;

public partial class ShopCard : Control
{
	public Label PriceLabel;
	public Sprite2D ItemSprite;
	
	public Item Item;
	
	public int Speed = 8;
	public string Direction = "up";
	
	public int Steps = 0;
	public bool FinishedAnimation = true;
	
	public override void _Ready()
	{
		PriceLabel = GetNode<Label>("PriceLabel");
		ItemSprite = GetNode<Sprite2D>("ItemSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!FinishedAnimation) {
			AnimatePurchase();
		}
	}
	
	public void AnimatePurchase() {
		if (Direction == "up") {
			Position = new Vector2(Position.X, Position.Y - Speed);
			Steps++;
			if (Steps >= 10) {
				Direction = "down";
				Steps = 0;
			}
		} else if (Direction == "down") {
			Position = new Vector2(Position.X, Position.Y + Speed);
			Steps++;
			if (Steps >= 10) {
				FinishedAnimation = true;
				Steps = 0;
				Direction = "up";
			}
		}
		
	}
}
