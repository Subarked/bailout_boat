using Godot;
using System;

public partial class BreakingPointBehaviour : InteractableObject
{
	[Export]
	public bool broken = false;
	[Export]
	public double Length;
	[Export]
	public double damage = 0;
	[Export]
	private float PixelWidth = 8;
	private RoomBehaviour Room;
	private Sprite2D sprite2D;
	private ShaderMaterial Shader;
	private Vector2[] crackingPoints;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = FindChild("Sprite2D") as Sprite2D;
		Room = GetParent() as RoomBehaviour;

		Shader = new ShaderMaterial() { Shader = (sprite2D.Material as ShaderMaterial).Shader.Duplicate() as Shader };
		sprite2D.Material = Shader;
		uint armNumber = GD.Randi() % 3 + 3;
		crackingPoints = new Vector2[1+armNumber*2];
		crackingPoints[0] = Vector2.Zero;
		for (int i = 0; i < armNumber; i++) {
			float areaOfRandom = (2f * Mathf.Pi)/armNumber;
			float firstAngle = (float)GD.RandRange(areaOfRandom*i, areaOfRandom*(i+1));
			float secondAngle = (float)GD.RandRange(-areaOfRandom/2f, areaOfRandom/2f)+firstAngle;
			Vector2 firstJoint = new Vector2(MathF.Sin(firstAngle), MathF.Cos(firstAngle))*0.5f;
			Vector2 secondJoint = new Vector2(MathF.Sin(secondAngle), MathF.Cos(secondAngle))*0.5f;
			float armLength = (firstJoint+secondJoint).Length();
			firstJoint *= (1f/armLength);
			secondJoint *= (1f/armLength);
			crackingPoints[1+i*2] = firstJoint;
			crackingPoints[1+i*2+1] = secondJoint;
		}
		Shader.SetShaderParameter("Points", Variant.CreateFrom(crackingPoints));
		Shader.SetShaderParameter("PointCount", Variant.CreateFrom(crackingPoints.Length));
		Shader.SetShaderParameter("PixelWidth", Variant.CreateFrom(PixelWidth));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (broken)
		{
			double ignore = Room.Volume;
			WaterMover.MoveWater(ref ignore, Room.Height, Room.Volume, ref Room.WaterVolume, Room.Height, Room.Volume, Length * damage, delta);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		if (broken)
		{
			sprite2D.Visible = true;
			damage = Math.Clamp(damage + delta / 15d, 0d, 1d);
			Shader.SetShaderParameter("Damage", Variant.CreateFrom(damage));
		}
		else
		{
			sprite2D.Visible = false;
		}
		
	}

	public override int Interacted(Player player, bool altInteracted)
	{
		if (broken)
		{
			//GD.Print("aaaa!");
			damage = damage - 10d / 60d;
			if (damage <= 0)
			{
				damage = 0;
				broken = false;
			}
			return 1;
		}
		else if (altInteracted)
		{
			broken = true;
			return 1;
		}
		else
		{
			return -1;
		}

	}
}