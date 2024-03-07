using Godot;
using System;

public partial class PumpBehaviour : ToggleInteractableObject
{
	[Export]
	public float PumpWidth;
	private RoomBehaviour Room;
	private Sprite2D sprite;
	float TransferSpeed;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = FindChild("Sprite2D") as Sprite2D;
		Room = GetParent() as RoomBehaviour;
		TransferSpeed = PumpWidth * 20f * 5f; //I swear I'll learn fluid dynamics soon please bear with me
	}

	public override int SwitchedState(Player player)
	{
		sprite.Frame = base.State ? 0 : 1;
		////Expanded ternary (?:) operator
		//if (State) //if it's open now, it means it was closed before so we...
		//{
		//	sprite.Frame = 0; //...change to the open frame
		//} else { //if it's closed now, it means it was open before so we....
		//	sprite.Frame = 1; //...change to the closed frame
		//}
		return 1;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (State)
		{
			//I swear I'll learn fluid dynamics soon please bear with me
			float pressure = Mathf.Clamp(Room.Area / Room.FloodAmount, 0, 1); //Completely bullshit pressure calc
			float TransferAmount = Mathf.Clamp(Room.FloodAmount, -TransferSpeed * (float)delta, TransferSpeed * (float)delta); //Amount to transfer from Room0 to NULL, negative if transferring from NULL to Room0
			Room.FloodAmount -= TransferAmount * pressure; //Change Room0 water amounts, Multiply by pressure to make it slower at lower amounts
		}
	}
}
