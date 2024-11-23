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
		
		switch (PointerPosition) { //in ortder to not hardcode this all you havbe to do is refrence the shopcards positions 
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
					switch (purchasedItem.Effect) {
						case "Heal":
							Arena.HealPlayer((float)purchasedItem.Amount);
							break;
						case "Stamina":
							Arena.RefillPlayerStamina((float)purchasedItem.Amount);
							break;
						case "FlowerSpawnRate":
							Arena.IncreaseFlowerSpawnRate((float)purchasedItem.Amount);
							break;
						case "PlusHeart":
							Arena.PlusPlayerHeart((float)purchasedItem.Amount);
							break;
						case "PlusStamina":
							Arena.PlusPlayerStamina((float)purchasedItem.Amount);
							break;
						case "PlusSpeed":
							Arena.PlusPlayerSpeed((float)purchasedItem.Amount);
							break;
					}

				}
			} else {
				//GetTree().Paused = false;
				Arena.ProcessMode = Node.ProcessModeEnum.Pausable;
				QueueFree();
			}
		}
	}
}
