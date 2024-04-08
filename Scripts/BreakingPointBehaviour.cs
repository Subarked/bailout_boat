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
	private RoomBehaviour Room;
	private Sprite2D sprite2D;
	private ShaderMaterial Shader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = FindChild("Sprite2D") as Sprite2D;
		Room = GetParent() as RoomBehaviour;

		Shader = new ShaderMaterial() { Shader = (sprite2D.Material as ShaderMaterial).Shader.Duplicate() as Shader };
		sprite2D.Material = Shader;
		Vector2[] crackingPoints = new Vector2[5];
		crackingPoints[0] = Vector2.Zero;
		for (int i = 1; i < crackingPoints.Length; i++) {
			crackingPoints[i] = new Vector2((float)GD.RandRange(-1f,1f), (float)GD.RandRange(-1f,1f));
		}
		Shader.SetShaderParameter("Points",Variant.CreateFrom(crackingPoints));
		Shader.SetShaderParameter("PointCount", Variant.CreateFrom(crackingPoints.Length));
		Shader.SetShaderParameter("PixelWidth", Variant.CreateFrom(16));
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
			damage = Math.Clamp(damage + delta / 60d, 0d, 1d);
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