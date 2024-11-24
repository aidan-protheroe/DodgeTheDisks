using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

public partial class Main : Node
{
	private Arena Arena;
	
	private PackedScene ArenaScene;
	private PackedScene PauseMenuScene;
	private PackedScene RestartMenuScene;
	
	private PauseMenu? pm = null;
	private RestartMenu rm;
	
	private CanvasLayer cv;
	
	private bool GameOver = false;
	private bool HandledGameOver = false;
	
	public UserData UserData;
	public string UserDataFilePath = "data/UserData.json";
	
	public override void _Ready()
	{
		Arena = GetNode<Arena>("Arena"); 
		ArenaScene = GD.Load<PackedScene>("res://scenes/Arena.tscn");
		PauseMenuScene = GD.Load<PackedScene>("res://scenes/PauseMenu.tscn");
		RestartMenuScene = GD.Load<PackedScene>("res://scenes/RestartMenu.tscn");
		cv = GetNode<CanvasLayer>("CanvasLayer");
		
		LoadUserData();


	}

	public override void _Process(double delta)
	{
		if (!GameOver) {
			if (Input.IsActionJustPressed("escape")) {
				if (pm == null) {
					pm = (PauseMenu)PauseMenuScene.Instantiate();
					cv.AddChild(pm);
					GetTree().Paused = true;
				}
			}
			
			if (pm != null) {
				if (!pm.Active) {
					pm.QueueFree();
					pm = null;
					GetTree().Paused = false;
				}
			}
			
			GameOver = Arena.GameOver;
		} else {
			if (!HandledGameOver) {
				HandledGameOver = true;
				var Score = Arena.TotalTime;
				if (Score > UserData.HighScore) {
					UserData.HighScore = Score;
					SaveUserData();
				}
				Arena.QueueFree();
				rm = (RestartMenu) RestartMenuScene.Instantiate(); //show score on restart screen (and if it's a new highscore highlight that)
				cv.AddChild(rm);
			} else {
				if (rm.NewGame) {
					rm.QueueFree();
					Arena = (Arena)ArenaScene.Instantiate();
					AddChild(Arena);
					GameOver = false;
					HandledGameOver = false;
				}
			}
		}

	}
	
	private void LoadUserData() {
		string jsonString = File.ReadAllText(UserDataFilePath);
		UserData = JsonSerializer.Deserialize<UserData>(jsonString)!;
	}
	
	private void SaveUserData() {
		string jsonString = JsonSerializer.Serialize(UserData);
		File.WriteAllText(UserDataFilePath, jsonString);
	}
}

public class UserData {
	public float HighScore { get; set; }
	public int Points { get; set; }
}

//add some animtaions to the shop so the player can tell when an upgrade is purchased vs when they don't have enough money
//disable shop cards? or just let the player buy as many as they want?

//after little playtest:
	//add weight to items(heartx1 and staminax.5 more common than others)
	//reduce prices overall, calculate average flowers per 100s for several games to determine correct range
	//animations for shop 
	//when buying something replace it with another randomly selected item?
	//store user data(highscore, points, etc)
	//should only get a max of 1 of each type of item during a shop selection, no heart x1 and heart x3 in the same selection
	//add an animation to disks for when they are destroyed(breaking apart into pieces??) and maybe a white flash over the entire screen for just a second when the player is hit
	//rare chance for heart to appear instead of flower

//add flags for how many disks can spawn at the same time(1-4, maybe more)
//two kind sof currency - one for during game, one for in hub to purchase upgrades
//add a MaxDisksOnScreen var that can increase with difficulty
//or potientally have an algorithm that can determine how many can be on screen deopending on the total comvined size of the disks that are available

//give the DiskGenerator a point system, where it can spend points(the max depends on difficulty) to give each variable 
//you can buy equipment in the store outside of the main loop and can equip 3 items at a time? stuff like slower disks, extera health/stamina, faster speed, etc
//add more paths as difficulty increases(maybe just start with 2?)

//equipemt rthat slows down time when dashing

//can purtchaswe extra equipment slots (maybe only in game loop with coins? To make it harder? )

//add max size and max speed to disks, focus more on  adding more disks at a certain point

//active itemd can be used with space, like clearing all disks on screen

//add several exports(maybe in a autoload) for debugging, like game speed, etc. Eventauly add a debug menu in-game maybe?

//remove all flowers when player gets hit, just like disks?
//rare chance for a chest to spawn, giving a free item?
//either have flower collisions work like disk collision or vice versa

//UPGRADE IDEAS(apply points to upgrade, permanent):
//Health+ 
//Stamina+ 
//Speed+

//PASSIVE ITEM IDEAS(acquired inside arena):
//increase flower spawn rate
//increase flower lifetime
//decrease disk spawn rate or size or speed
//Health+
//Stamina+
//Speed+
//survive death once

//ACTIVE ITEM IDEAS(acquired and used inside arena, can be used on purhcase or 1 can be held):
//Destroy all disks on screen
//refill health(comes in 1 or 3 variantsd)
//refill stamina(comes in 1/2 or full variants)
//flower spawn rate increase signifigantly for a set time
