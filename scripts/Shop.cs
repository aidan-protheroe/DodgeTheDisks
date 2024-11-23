using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Shop : Control
{
	public Random rnd;
	
	public ShopCard ShopCard1;
	public ShopCard ShopCard2;
	public ShopCard ShopCard3;
	
	public Array<ShopCard> ShopCards;
	
	public int PlayerFlowers;
	
	public AnimatedSprite2D Pointer;
	public int PointerPosition = 0;
	
	public Arena Arena;
	
	public bool Finished = false;
	
	public void init(Arena arena) {
		
		Arena = arena;
		
		Pointer = GetNode<AnimatedSprite2D>("Pointer");
		Pointer.Play();
		
		ShopCards = new Array<ShopCard>() {
			GetNode<ShopCard>("ShopCard"),
			GetNode<ShopCard>("ShopCard2"),
			GetNode<ShopCard>("ShopCard3")
		};
		
		rnd = new Random();
		
		for (int i = 0 ; i < 3 ; i++) {
			var item = Arena.AvailableItems[rnd.Next(Arena.AvailableItems.Count)];
			Arena.AvailableItems.Remove(item);
			ShopCards[i].Item = item;
			ShopCards[i].PriceLabel.Text = "x" + item.Price;
			ShopCards[i].ItemSprite.Texture = item.Sprite;
		}
	}
	
	public override void _Ready()
	{
		//temp
		//var i = new ItemLoader();
		//var x = i.LevelOneItems;
		//init(x, 10);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("left")) {
			PointerPosition--;
		} else if (Input.IsActionJustPressed("right")) {
			PointerPosition++;
		}
		if (PointerPosition < 0) {
			PointerPosition = 3;
		} else if (PointerPosition > 3) {
			PointerPosition = 0;
		}
		
		switch (PointerPosition) {
			case 0: 
				Pointer.Position = new Vector2(521, 496);
				break;
			case 1:
				Pointer.Position = new Vector2(788, 496);
				break;
			case 2:
				Pointer.Position = new Vector2(1095, 496);
				break;
			case 3:
				Pointer.Position = new Vector2(798, 783);
				break;
		}
		
		if (Input.IsActionJustPressed("enter")) {
			if (PointerPosition != 3) { //add check to make sure player health/stamina isn't full before purchasing somehow
				if (Arena.Player.Flowers >= ShopCards[PointerPosition].Item.Price) {
					Arena.Player.Flowers -= ShopCards[PointerPosition].Item.Price;
					Arena.FlowerLabel.Text = "x" + Arena.Player.Flowers;
					
					var purchasedItem = ShopCards[PointerPosition].Item;
					if (purchasedItem.Effect == "Heal") {
						Arena.HealPlayer((float)purchasedItem.Amount);
					} else if (purchasedItem.Effect == "Stamina") {
						Arena.RefillPlayerStamina((float)purchasedItem.Amount);
					}
				}
			} else {
				Arena.Player.Flowers = PlayerFlowers;
				GetTree().Paused = false;
				QueueFree();
			}
		}
	}
}
