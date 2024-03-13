using Godot;
using System;

public partial class InteractableObject : Node2D
{
	[Export]
	public double maxInteractDistance = 35.77708764;
	public int Interact(Player player, bool altInteracted)
	{
		if (player.GlobalPosition.DistanceTo(this.GlobalPosition) <= maxInteractDistance)
		{
			return Interacted(player, altInteracted);
		}
		return 0;
	}

	public virtual int Interacted(Player player, bool altInteracted)
	{
		return 1;
	}
}