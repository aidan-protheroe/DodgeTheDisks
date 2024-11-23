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
	
	public List<Item> PurchasedItems;
	
	public int PlayerFlowers;
	
	public AnimatedSprite2D Pointer;
	public int PointerPosition = 0;
	
	public Arena Arena;
	
	public bool Finished = false;
	
	public void init(List<Item> AllItems, int playerFlowers, Arena arena) {
		
		Arena = arena;
		
		PlayerFlowers = playerFlowers;
		
		Pointer = GetNode<AnimatedSprite2D>("Pointer");
		Pointer.Play();
		
		ShopCards = new Array<ShopCard>() {
			GetNode<ShopCard>("ShopCard"),
			GetNode<ShopCard>("ShopCard2"),
			GetNode<ShopCard>("ShopCard3")
		};
		
		rnd = new Random();
		
		PurchasedItems = new List<Item>();
		
		
		for (int i = 0 ; i < 3 ; i++) {
			var item = AllItems[rnd.Next(AllItems.Count)];
			AllItems.Remove(item);
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
				if (PlayerFlowers >= ShopCards[PointerPosition].Item.Price) {
					PlayerFlowers -= ShopCards[PointerPosition].Item.Price;
					Arena.FlowerLabel.Text = "x" + PlayerFlowers;
					PurchasedItems.Add(ShopCards[PointerPosition].Item);
					
					var purchasedItem = ShopCards[PointerPosition].Item;
					if (purchasedItem.Effect == "Heal") {
						Arena.HealPlayer((float)purchasedItem.Amount);
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
