using Godot;
using System;

public class WaterMover
{
	public static void MoveWater(ref double Room0WaterVolume, in double Room0Height, in double Room0Volume, ref double Room1WaterVolume, in double Room1Height, in double Room1Volume, double InletLength, double delta, bool isOnWall = false)
	{
		double dischargeCoefficient = 0.62d * 0.97d;
		double flowConstant = 2d / 3d * dischargeCoefficient;
		double gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsDouble();
		double WaterHeight0 = Math.Max(isOnWall ? Room0Volume / Room0WaterVolume * Room0Height - InletLength / 2d : Room0Volume / Room0WaterVolume * Room0Height, 0d);
		double WaterHeight1 = Math.Max(isOnWall ? Room1Volume / Room1WaterVolume * Room1Height - InletLength / 2d : Room1Volume / Room1WaterVolume * Room1Height, 0d);
		double Room0VolumeFlow = flowConstant * InletLength * Math.Pow(2d * gravity * WaterHeight0, 0.5d);
		double Room1VolumeFlow = flowConstant * InletLength * Math.Pow(2d * gravity * WaterHeight1, 0.5d);
		double totalVolumeFlow = Room0VolumeFlow - Room1VolumeFlow;
		GD.Print(WaterHeight0);
		GD.Print(WaterHeight1);
		GD.Print(Room0VolumeFlow);
		GD.Print(Room1VolumeFlow);
		GD.Print(totalVolumeFlow);
		GD.Print("");
		Room0WaterVolume -= totalVolumeFlow * delta;
		Room1WaterVolume += totalVolumeFlow * delta;
	}
}