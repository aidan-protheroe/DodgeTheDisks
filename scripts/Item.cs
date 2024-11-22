using Godot;
using System;
using System.Collections.Generic;

public class Item
{
	public string Name {get; set;}
	public Texture2D? Sprite {get; set;}
	public int Price {get; set;}
	public string Effect {get; set;} 
	public float? Amount {get; set;}
}


public class ItemLoader {
	public List<Item> LevelOneItems;
	public List<Item> LevelTwoItems;
	public List<Item> LevelThreeItems;
	
	public ItemLoader() {
		LevelOneItems = new List<Item>() {
			new Item() {
				Name = "SingleHeart",
				Sprite = null,
				Price = 5,
				Effect = "Heal",
				Amount = 1f
			},
			new Item() {
				Name = "HalfStamina",
				Sprite = null,
				Price = 8,
				Effect = "Stamina",
				Amount = .5f
			},
			new Item() {
				Name = "PlusFlowers",
				Sprite = null,
				Price = 12,
				Effect = "FlowerSpawnRate",
				Amount = 1.25f
			}
		};
		LevelTwoItems = new List<Item>() {
			new Item() {
				Name = "TripleHeart",
				Sprite = null,
				Price = 12,
				Effect = "Heal",
				Amount = 3f
			},
			new Item() {
				Name = "PlusHeart",
				Sprite = null,
				Price = 16,
				Effect = "PlusHeart",
				Amount = 1f
			},
			new Item() {
				Name = "PlusStamina",
				Sprite = null,
				Price = 16,
				Effect = "PlusStamina",
				Amount = 1.25f
			},
			new Item() {
				Name = "PlusSpeed",
				Sprite = null,
				Price = 20,
				Effect = "PlusSpeed",
				Amount = 1.25f
			}
		};
		LevelThreeItems = new List<Item>() {
			new Item() {
				Name = "AllHearts",
				Sprite = null,
				Price = 24,
				Effect = "Heal",
				Amount = 0f
			},
			new Item() {
				Name = "FullStamina",
				Sprite = null,
				Price = 18,
				Effect = "Stamina",
				Amount = 1f
			},
			new Item() {
				Name = "LongerFlowerLife",
				Sprite = null,
				Price = 22,
				Effect = "LongerFlowerLife",
				Amount = 1.5f
			}
		};
		
	}
}
