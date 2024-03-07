using System;
using Godot;

public partial class Player : CharacterBody2D
{
	public const float HorizontalMovementSpeed = 300.0f;
	public const float JumpVelocity = -400.0f;
	[Export]
	public double ToppedOffOxygenTime = 30;
	[Export]
	public double OxygenTime = 30;
	[Export]
	public double MaxHealth = 100;
	[Export]
	public double Health = 100;
	
	private Node2D sprite;
	private ShapeCast2D PlatfornDetect;
	private Area2D InteractArea;
	private Area2D RoomDetect;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravityIntensity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public Vector2 gravityVector = ProjectSettings.GetSetting("physics/2d/default_gravity_vector").AsVector2();
	public Vector2 gravity;
	public Vector2 startingPositon;
	public bool isFlying = false;
	public bool isUnderwater = false; //o no!
	private ulong DrowningTimeStart = 0;
	public override void _Ready()
	{
		sprite = FindChild("Sprite2D") as Node2D;
		PlatfornDetect = FindChild("PlatformDetect") as ShapeCast2D;
		RoomDetect = FindChild("CameraChangeDetect") as Area2D;
		InteractArea = FindChild("InteractArea") as Area2D;

		startingPositon = Position;
		gravity = gravityVector * gravityIntensity;
		RoomDetect.AreaEntered += CameraAreaEntered;
	}
	//see if interact button was pressed and interact if so
	void TryInteract()
	{
		bool buttonPressed = Input.IsActionJustPressed("interact");
		if (buttonPressed)
		{
			if (InteractArea.HasOverlappingAreas())
			{
				var OverlappingAreas = InteractArea.GetOverlappingAreas();
				foreach (var area in OverlappingAreas)
				{
					Node interactedNode = area.GetParent();
					if (interactedNode is InteractableObject) {
						InteractableObject interacted = interactedNode as InteractableObject;
						interacted.Interact(this);
					}
				}
			}
		}
		//bool altInteracted = Input.IsActionJustPressed("interact_alt");
		//if (altInteracted)
		//{
		//	//TODO: FIND A USE
		//}
	}

	void CameraAreaEntered(Area2D area)
	{
		Globals.CameraCenter = area.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		

		if (isUnderwater)
		{
			OxygenTime -= delta;

			if (OxygenTime <= 0) {
				Health -= delta*20;
			}
		} else {
			OxygenTime += delta*3;
		}
		OxygenTime = Math.Clamp(OxygenTime,0,ToppedOffOxygenTime);
		Health = Math.Clamp(Health,0,MaxHealth);
		Vector2 velocity = Velocity;

		if (velocity.X < 0)
		{
			sprite.Scale = new Vector2(-1, 1);
		}
		if (velocity.X > 0)
		{
			sprite.Scale = new Vector2(1, 1);
		}
		// Add the gravity.
		if (!IsOnFloor() && !isFlying)
			velocity += gravity * (float)delta;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
		if (!isFlying)
		{
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * HorizontalMovementSpeed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, HorizontalMovementSpeed);
			}
		}
		else
		{
			if (direction != Vector2.Zero)
			{
				velocity = direction * HorizontalMovementSpeed;
			}
			else
			{
				velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, HorizontalMovementSpeed), Mathf.MoveToward(Velocity.Y, 0, HorizontalMovementSpeed));
			}
		}

		if (direction.Y > 0 && IsOnFloor() && PlatfornDetect.IsColliding())
		{
			Position = Position + gravityVector;
		}

		if (Position.Y > 1000)
		{
			Position = startingPositon;
		}

		if (Input.IsActionJustPressed("dev_fly"))
		{
			isFlying = !isFlying;
		};

		if (RoomDetect.HasOverlappingAreas()) //Drowning! Yippee!
		{
			var OverlappingAreas = RoomDetect.GetOverlappingAreas();
			foreach (var area in OverlappingAreas)
			{
				if (area.Name == "Room_Area")
				{
					Node roomNode = area.GetParent();
					if (roomNode != null)
					{
						RoomBehaviour room = roomNode as RoomBehaviour;
						if (GlobalPosition.Y - 12f >= room.WaterHeightYPosition)
						{
							if (!isUnderwater)
							{
								isUnderwater = true;
								DrowningTimeStart = Time.GetTicksMsec();
							}
						}
						else
						{
							if (isUnderwater)
							{
								isUnderwater = false;
								DrowningTimeStart = 0;
							}

						}
					}
				}
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		TryInteract();
	}
}
