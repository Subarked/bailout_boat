using Godot;
using System;

public partial class RoomBehaviour : Node2D
{
	[Export]
	public double FloodAmount;
	[Export]
	public float Area;
	[Export]
	public float Height;
	[Export]
	public float Width;
	[Export]
	public float WaterHeight;
	[Export]
	public float WaterHeightYPosition;
	private ShaderMaterial Shader;
	private float FillAmount;
	private Node2D RoomWater;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RoomWater = FindChild("Room Water") as Node2D;
		//THIS IS DUMB, I HATE THAT I HAVE TO DO THIS, WHYYYY
        Shader = new ShaderMaterial() { Shader = (RoomWater.Material as ShaderMaterial).Shader.Duplicate() as Shader };
		RoomWater.Material = Shader;
	}

    public override void _PhysicsProcess(double delta)
    {
        FloodAmount= Mathf.Min(FloodAmount,Area);
		FillAmount = (float)FloodAmount/Area;
		WaterHeight = FillAmount*Height;
		WaterHeightYPosition = GlobalPosition.Y+Height/2f-WaterHeight;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		//GD.Print(FillAmount);
		Shader.SetShaderParameter("FillAmount", Variant.CreateFrom(FillAmount));	
	}
}
