using Godot;
using System;

public partial class RoomBehaviour : Node2D
{
	[Export]
	public double WaterVolume;
	public double Volume;
	[Export]
	public double Height;
	[Export]
	public double Width;
	[Export]
	public double WaterHeight;
	[Export]
	public double WaterHeightYPosition;
	private ShaderMaterial Shader;
	private double FillAmount;
	private Node2D RoomWater;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RoomWater = FindChild("Room Water") as Node2D;
		//THIS IS DUMB, I HATE THAT I HAVE TO DO THIS, WHYYYY
		Shader = new ShaderMaterial() { Shader = (RoomWater.Material as ShaderMaterial).Shader.Duplicate() as Shader };
		RoomWater.Material = Shader;
		Volume = Height * Width;
	}

	public override void _PhysicsProcess(double delta)
	{
		WaterVolume = Mathf.Min(WaterVolume, Volume);
		FillAmount = (float)WaterVolume / Volume;
		WaterHeight = FillAmount * Height;
		WaterHeightYPosition = GlobalPosition.Y + Height / 2f - WaterHeight;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(FillAmount);
		Shader.SetShaderParameter("FillAmount", Variant.CreateFrom(FillAmount));
	}
}
