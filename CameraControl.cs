using Godot;
using System;

public partial class CameraControl : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 RoundedCameraCenter = Globals.CameraCenter.Round()+Vector2.One*0.1f;
		if (RoundedCameraCenter != Position) {
			Position = RoundedCameraCenter;
		}
	}
}
