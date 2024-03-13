using Godot;
public partial class ToggleInteractableObject : InteractableObject
{
	[Export]
	public bool State = false;
	public override int Interacted(Player player, bool altInteracted) {
		State = !State;
		
		return SwitchedState(player, altInteracted);
	}
	//was toggled
	public virtual int SwitchedState(Player player, bool altInteracted) {
		return 1;
	}
}