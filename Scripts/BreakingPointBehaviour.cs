using Godot;
using System;

public partial class Breaking : InteractableObject
{
	[Export]
	public bool broken = false;
	public double damage = 0;
	private Sprite2D sprite2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = FindChild("Sprite2D") as Sprite2D;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (broken)
		{
			damage = Math.Clamp(damage + delta / 60d, 0d, 1d);
			sprite2D.Frame = (int)(damage * (sprite2D.Vframes * sprite2D.Hframes - 1));
		}

	}

	public override int Interacted(Player player)
	{
		GD.Print("aaaa!");
		damage = Math.Clamp(damage - 10d / 60d, 0d, 1d);
		sprite2D.Frame = (int)(damage * (sprite2D.Vframes * sprite2D.Hframes - 1));
		return 1;
	}
}