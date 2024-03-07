using Godot;
using System;

public partial class StairsBehaviour : InteractableObject
{
	[Export]
	public StairsBehaviour ConnectedStairs;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (ConnectedStairs != null)
		{
			ConnectedStairs.ConnectedStairs = this;
		}
	}
	
	public override int Interacted(Player player)
	{
		//Transform2D startingTransform = Transform;
		//Transform2D endingTransform = ConnectedStairs.Transform;
		//Transform2D transformInverse = Transform.AffineInverse();
		Vector2 startingPosition = GlobalPosition;
		Vector2 endingPosition = ConnectedStairs.GlobalPosition;
		Vector2 initialOffset = player.GlobalPosition - startingPosition;
		player.GlobalPosition = endingPosition + initialOffset;
		return 1;
	}
}
