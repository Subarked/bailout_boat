using Godot;
using System;

public partial class CameraControl : Node2D
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 RoundedCameraCenter = Globals.CameraCenter.Round()+Vector2.One*0.1f;
		if (RoundedCameraCenter != Position) {
			Position = RoundedCameraCenter;
		}
	}
}
