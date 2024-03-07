using Godot;
using System;

public partial class PumpBehaviour : Node2D
{
	[Export]
	public bool IsOpen = false;
	[Export]
	public float PumpWidth;
	private RoomBehaviour Room;
	private bool prevIsOpen = true;
	private Sprite2D sprite;
	float TransferSpeed;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = FindChild("Sprite2D") as Sprite2D;
		Room = GetParent() as RoomBehaviour;
		TransferSpeed = PumpWidth * 20f*5f; //these numbers are complete bullshit, I just thought they looked good
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsOpen)
		{
			if (!prevIsOpen)
			{
				sprite.Frame = 0;
			}
			float difference = Room.FloodAmount; //Hangover from stealing the code from DoorBehaviour
			float pressure = Mathf.Clamp(Room.Area/difference,0,1); //Completely bullshit pressure calc
			float TransferAmount = Mathf.Clamp(difference, -TransferSpeed * (float)delta, TransferSpeed * (float)delta); //Amount to transfer from Room0 to Room1, negative if transferring from Room1 to Room0
			Room.FloodAmount -= TransferAmount*pressure; //Change Room0 water amounts, Multiply by pressure to make it slower at lower amounts
		}
		if (!IsOpen)
		{
			if (prevIsOpen)
			{
				sprite.Frame = 1;
			}
		}
		prevIsOpen = IsOpen;
	}
}
