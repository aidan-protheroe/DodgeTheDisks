using Godot;
using System;

public partial class ShopCard : Control
{
	public Label PriceLabel;
	public Sprite2D ItemSprite;
	
	public Item Item;
	
	public override void _Ready()
	{
		PriceLabel = GetNode<Label>("PriceLabel");
		ItemSprite = GetNode<Sprite2D>("ItemSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
