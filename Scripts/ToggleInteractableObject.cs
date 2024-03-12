using Godot;
public partial class ToggleInteractableObject : InteractableObject
{
	[Export]
	public bool State = false;
	public override int Interacted(Player player) {
		State = !State;
		
		return SwitchedState(player);
	}
	//was toggled
	public virtual int SwitchedState(Player player) {
		return 1;
	}
}