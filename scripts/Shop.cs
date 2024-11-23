using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Shop : Control
{
	public Random rnd;
	
	public Label ExitLabel;
	
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
		
		ExitLabel = GetNode<Label>("ExitLabel");
	}
	
	public override void _Ready()
	{
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
		
		//i dont think you even need the switch anymore, just use a one liner here basically
		//switch (PointerPosition) { //in ortder to not hardcode this all you havbe to do is refrence the shopcards positions 
			//case 0: 
				//Pointer.Position = new Vector2(ShopCards[0].Position.X + (ShopCards[0].Size.X / 2), ShopCards[0].Position.Y + ShopCards[0].Size.Y + 25);
				//break;
			//case 1:
				//Pointer.Position = new Vector2(788, ShopCards[1].Position.Y + ShopCards[1].Size.Y + 25);
				//break;
			//case 2:
				//Pointer.Position = new Vector2(1095, ShopCards[2].Position.Y + ShopCards[2].Size.Y + 25);
				//break;
			//case 3:
				//Pointer.Position = new Vector2(798, ExitLabel.Position.Y + ExitLabel.Size.Y + 25);
				//break;
		//}
		
		if (PointerPosition != 3) {
			Pointer.Position = new Vector2(ShopCards[PointerPosition].Position.X + (ShopCards[PointerPosition].Size.X / 2), ShopCards[PointerPosition].Position.Y + ShopCards[PointerPosition].Size.Y + 25);
		} else {
			Pointer.Position = new Vector2(798, ExitLabel.Position.Y + ExitLabel.Size.Y + 25);
		}
		
		if (Input.IsActionJustPressed("enter")) {
			if (PointerPosition != 3) { //add check to make sure player health/stamina isn't full before purchasing somehow
				if (Arena.Player.Flowers >= ShopCards[PointerPosition].Item.Price && ShopCards[PointerPosition].FinishedAnimation) {
					Arena.Player.Flowers -= ShopCards[PointerPosition].Item.Price;
					Arena.FlowerLabel.Text = "x" + Arena.Player.Flowers;
					
					ShopCards[PointerPosition].FinishedAnimation = false; //test
					
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
				Arena.ProcessMode = Node.ProcessModeEnum.Pausable;
				QueueFree();
			}
		}
	}
}
