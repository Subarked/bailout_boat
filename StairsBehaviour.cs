using Godot;
using System;

public partial class StairsBehaviour : Node2D
{
	[Export]
	public StairsBehaviour ConnectedStairsBehaviour;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (ConnectedStairsBehaviour != null) {
			ConnectedStairsBehaviour.ConnectedStairsBehaviour = this;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}
}
