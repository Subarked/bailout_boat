using Godot;
public partial class ToggleInteractableObject : InteractableObject
{
	[Export]
	public bool State = false;
	public override int Interact(Player player) {
		State = !State;
		
		return SwitchedState(player,State);
	}
	//was toggled
	public virtual int SwitchedState(Player player) {
		return 1;
	}
}