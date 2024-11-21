using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}


//add several different kinds of disks(size, speed, bouncing, target)
//add time flags for what kind of disks can be spawned (at 1 min spawn x and y, and 2 min spawn z and a, etc)
//add flags for how many disks can spawn at the same time(1-4, maybe more)
//Every 2 minutes a shop appears? for things run only
//two kind sof currency - one for during game, one for in hub to purchase upgrades
//add groupings of DiskData's that are ranked from easy to hard-- and select a random one of these groupings to be used every 30-60 seconds
//ie the starting group of DiskData can be (bigRandom, regularPlayer, and smallFast) or (bigPlayer, regularFast, and smallRandom) etc etc
//maybe eventually just randomize every single aspect of the disks, with ddifferent ranges and options available as duifficulty increases
//add a MaxDisksOnScreen var that can increase with difficulty
//or potientally have an algorithm that can determine how many can be on screen deopending on the total comvined size of the disks that are available

//pass a range into loadgenerator for numbe rof disk types to generate
//add a CD disk

//give the DiskGenerator a point system, where it can spend points(the max depends on difficulty) to give each variable 
//you can buy equipment in the store outside of the main loop and can equip 3 items at a time? stuff like slower disks, extera health/stamina, faster speed, etc
//remove offscreen disks
//add coins
//add more paths as difficulty increases(maybe just start with 2?)

//equipemt rthat slows down time when dashing

//can purtchaswe extra equipment slots (maybe only in game loop with coins? To make it harder? )
//when you get hit all disks disapear

//do you even need to keep a list of disks?

//add max size and max speed to disks, focus more on  adding more disks at a certain point
//give stamina bar same aesthetic as hearts, white outline white fill

//active itemd can be used with space, like clearing all disks on screen

//do heartGrid size changes when numOfHearts % 3 = 1, add 1 heart height + buffer size to accomeadte

//add several exports(maybe in a autoload) for debugging, like game speed, etc. Eventauly add a debug menu in-game maybe?


//remove all flowers when player gets hit, just like disks?


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

//ACTIVE ITEM IDEAS(acquired and used inside arena, can be used on purhcase or 1 can be held):
//Destroy all disks on screen
//refill health(comes in 1 or 3 variantsd)
//refill stamina(comes in 1/2 or full variants)
//flower spawn rate increase signifigantly for a set time
