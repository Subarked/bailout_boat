using Godot;
using System;

public partial class InteractableObject : Node2D
{
	[Export]
	public double maxInteractDistance = Math.Pow(Math.Pow(32, 2) + Math.Pow(16, 2), 1d / 2d);
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