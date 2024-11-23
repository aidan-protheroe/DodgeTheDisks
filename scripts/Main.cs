using Godot;
using System;

public partial class Main : Node
{
	private PackedScene PauseMenuScene;
	private PauseMenu? pm = null;
	
	public override void _Ready()
	{
		PauseMenuScene = GD.Load<PackedScene>("res://scenes/PauseMenu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape")) {
			if (pm == null) {
				pm = (PauseMenu)PauseMenuScene.Instantiate();
				AddChild(pm);
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
	}
}

//get shop and pause menu to get along
//when adding a heart while they aren't all full it adds a new full one and keeps the old ones empty-- gotta fix


//add flags for how many disks can spawn at the same time(1-4, maybe more)
//Every 2 minutes a shop appears? for things run only
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

//do heartGrid size changes when numOfHearts % 3 = 1, add 1 heart height + buffer size to accomeadte

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
