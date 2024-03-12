using Godot;
using System;

public class WaterMover
{
	public static void MoveWater(ref double Room0WaterVolume, in double Room0Height, in double Room0Volume, ref double Room1WaterVolume, in double Room1Height, in double Room1Volume, double InletLength, double delta, bool isOnWall = false)
	{
		double dischargeCoefficient = 0.62d * 0.97d;
		double gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();
		double Room0WaterHeight = Room0WaterVolume / Room0Volume * Room0Height;
		double Room1WaterHeight = Room1WaterVolume / Room1Volume * Room1Height;
		//GD.Print(Room0WaterHeight);
		//GD.Print(Room1WaterHeight);
		double Room0VolumeFlow = 0;
		double Room1VolumeFlow = 0;

		if (isOnWall)
		{
			double Room0DoorTopWaterHeight = Math.Max(Room0WaterHeight - InletLength, 0d);
			double Room1DoorTopWaterHeight = Math.Max(Room1WaterHeight - InletLength, 0d);
			//GD.Print(Room0DoorTopWaterHeight);
			//GD.Print(Room1DoorTopWaterHeight);
			double Room0WaterHeight1 = Math.Pow(Room0DoorTopWaterHeight, 1.5d);
			double Room1WaterHeight1 = Math.Pow(Room1DoorTopWaterHeight, 1.5d);
			//GD.Print(Room0WaterHeight1);
			//GD.Print(Room1WaterHeight1);
			double Room0WaterHeight2 = Math.Pow(Room0WaterHeight, 1.5d);
			double Room1WaterHeight2 = Math.Pow(Room1WaterHeight, 1.5d);
			//GD.Print(Room0WaterHeight2);
			//GD.Print(Room1WaterHeight2);
			double Room0HeightFlow = Room0WaterHeight2 - Room0WaterHeight1;
			double Room1HeightFlow = Room1WaterHeight2 - Room1WaterHeight1;
			//GD.Print(Room0HeightFlow);
			//GD.Print(Room1HeightFlow);
			Room0VolumeFlow = 2d / 3d * dischargeCoefficient * 1d * Math.Pow(2d * gravity, 0.5d) * Room0HeightFlow;
			Room1VolumeFlow = 2d / 3d * dischargeCoefficient * 1d * Math.Pow(2d * gravity, 0.5d) * Room1HeightFlow;
		}
		else
		{
			Room0VolumeFlow = dischargeCoefficient * InletLength * Math.Pow(2d * gravity * Room0WaterHeight, 0.5d);
			Room1VolumeFlow = dischargeCoefficient * InletLength * Math.Pow(2d * gravity * Room1WaterHeight, 0.5d);
		}
		//GD.Print(Room0VolumeFlow);
		//GD.Print(Room1VolumeFlow);
		double totalVolumeFlow = Room0VolumeFlow - Room1VolumeFlow;
		//GD.Print(totalVolumeFlow);
		//GD.Print("");
		Room0WaterVolume = Math.Clamp(Room0WaterVolume + -totalVolumeFlow * delta, 0, Room0Volume);
		Room1WaterVolume = Math.Clamp(Room1WaterVolume + totalVolumeFlow * delta, 0, Room0Volume);

	}
}