using Godot;
using System;

public partial class CharacterInfo : Control
{
	[Export]
	public Panel HealthPanel;
	[Export]
	public Panel OxygenPanel;
	private Character character;
	float HealthMaxWidth;
	float OxygenMaxWidth;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		character = GetParent() as Character;
		HealthMaxWidth = HealthPanel.Size.X;
		OxygenMaxWidth = OxygenPanel.Size.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HealthPanel.Size = new Vector2((character.Health/character.MaxHealth)*HealthMaxWidth,HealthPanel.Size.Y);
		OxygenPanel.Size = new Vector2((character.OxygenTime/character.MaxOxygenTime)*OxygenMaxWidth,OxygenPanel.Size.Y);
	}
}
