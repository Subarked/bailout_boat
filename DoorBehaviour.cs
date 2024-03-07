using Godot;
using System;

public partial class DoorBehaviour : Node2D
{
	[Export]
	public RoomBehaviour Room0;
	[Export]
	public RoomBehaviour Room1;
	[Export]
	public float DoorHeight;
	[Export]
	public bool IsOpen = false;
	private bool prevIsOpen = false;
	private StaticBody2D collider;
	float TransferSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TransferSpeed = DoorHeight * 20f*5f;
		collider = FindChild("StaticBody2D") as StaticBody2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsOpen)
		{
			if (!prevIsOpen)
			{
				collider.ProcessMode = ProcessModeEnum.Disabled;
			}

			float difference = Room0.FloodAmount - Room1.FloodAmount; //Room0 > Room1 = +, Room0 < Room1 = -
			float pressure = Mathf.Clamp(Room0.Area/difference/2f+Room1.Area/difference/2f,0,1); //Completely bullshit pressure calc XD
			float TransferAmount = Mathf.Clamp(difference, -TransferSpeed * (float)delta, TransferSpeed * (float)delta); //Amount to transfer from Room0 to Room1, negative if transferring from Room1 to Room0
			Room0.FloodAmount -= TransferAmount*pressure; //Change Room0 water amounts, Multiply by pressure to make it slower at lower amounts
			Room1.FloodAmount += TransferAmount*pressure; //Change Room1 water amounts, Multiply by pressure to make it slower at lower amounts
		}
		if (!IsOpen)
		{
			if (prevIsOpen)
			{
				collider.ProcessMode = ProcessModeEnum.Inherit;
			}
		}

		prevIsOpen = IsOpen;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Modulate = IsOpen ? new Color(.5f, .5f, .5f) : new Color(1f, 1f, 1f);
	}
}
