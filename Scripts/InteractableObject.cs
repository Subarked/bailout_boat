using Godot;
using System;

public partial class InteractableObject : Node2D
{
	[Export]
	public double maxInteractDistance = 35.77708764;
	public int Interact(Player player)
	{
		if (player.GlobalPosition.DistanceTo(this.GlobalPosition) <= maxInteractDistance)
		{
			return Interacted(player);
		}
		return 0;
	}
	
	public virtual int Interacted(Player player)
	{
		return 1;
	}
}