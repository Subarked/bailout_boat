using Godot;
using System;

public partial class InteractableObject : Node2D
{
	public virtual int Interact(Player player) {
		return 1;
	}
}