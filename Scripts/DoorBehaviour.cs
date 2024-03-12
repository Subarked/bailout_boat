using Godot;
using System;

public partial class DoorBehaviour : ToggleInteractableObject
{
	[Export]
	public RoomBehaviour Room0;
	[Export]
	public RoomBehaviour Room1;
	[Export]
	public float DoorHeight;
	private StaticBody2D collider;
	float TransferSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TransferSpeed = DoorHeight * 20f * 5f;
		collider = FindChild("StaticBody2D") as StaticBody2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (State)
		{
			//These numbers don't make sense, I'll learn fluid dynamics eventually I promise
			double difference = Room0.FloodAmount - Room1.FloodAmount; //Room0 > Room1 = +, Room0 < Room1 = -
			double pressure = Math.Clamp(Room0.Area / Math.Abs(difference) / 2f + Room1.Area / Math.Abs(difference) / 2f, 0, 1); //Completely bullshit pressure calc XD
			double TransferAmount = Math.Clamp(difference, -TransferSpeed * delta, TransferSpeed * delta); //Amount to transfer from Room0 to Room1, negative if transferring from Room1 to Room0
			Room0.FloodAmount -= TransferAmount * pressure; //Change Room0 water amounts, Multiply by pressure to make it slower at lower amounts
			Room1.FloodAmount += TransferAmount * pressure; //Change Room1 water amounts, Multiply by pressure to make it slower at lower amounts
		}
	}
	public override int SwitchedState(Player player)
	{

		collider.ProcessMode = State ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
		////Expanded ternary (?:) operator
		//if (State) //if it's open now, it means it was closed before so we...
		//{
		//	collider.ProcessMode = ProcessModeEnum.Disabled; //...turn off the collision
		//} else { //if it's closed now, it means it was open before so we....
		//	collider.ProcessMode = ProcessModeEnum.Inherit; //...turn on the collision
		//}
		return 1;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Modulate = State ? new Color(.5f, .5f, .5f) : new Color(1f, 1f, 1f);
	}
}
