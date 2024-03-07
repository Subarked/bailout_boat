using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Runtime.InteropServices;

public partial class Character : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	[Export]
	public float MaxOxygenTime = 30000;
	[Export]
	public float OxygenTime = 30000;
	[Export]
	public float MaxHealth = 100;
	[Export]
	public float Health = 100;
	
	private Node2D sprite;
	private ShapeCast2D PlatfornDetect;
	private Area2D DoorDetect;
	private Area2D RoomDetect;
	private Area2D StairDetect;
	private Area2D PumpDetect;

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
		StairDetect = FindChild("StairDetect") as Area2D;
		PumpDetect = FindChild("PumpDetect") as Area2D;
		DoorDetect = FindChild("DoorDetect") as Area2D;

		startingPositon = Position;
		gravity = gravityVector * gravityIntensity;
		RoomDetect.AreaEntered += CameraAreaEntered;
	}

	void TryInteract()
	{
		bool interacted = Input.IsActionJustPressed("interact");
		if (interacted)
		{
			if (DoorDetect.HasOverlappingAreas())
			{
				var OverlappingAreas = DoorDetect.GetOverlappingAreas();
				foreach (var area in OverlappingAreas)
				{
					if (area.Name == "Door_Area")
					{
						Node doorNode = area.GetParent();
						if (doorNode != null)
						{
							DoorBehaviour door = doorNode as DoorBehaviour;
							door.IsOpen = !door.IsOpen;
						}
					}
				}
			}
			if (StairDetect.HasOverlappingAreas())
			{
				var OverlappingAreas = StairDetect.GetOverlappingAreas();
				foreach (var area in OverlappingAreas)
				{
					if (area.Name == "Stairs_Area")
					{
						Node stairsNode = area.GetParent();
						if (stairsNode != null)
						{
							StairsBehaviour stairs = stairsNode as StairsBehaviour;
							Vector2 startingPosition = stairs.GlobalPosition;
							Vector2 endingPosition = stairs.ConnectedStairsBehaviour.GlobalPosition;
							Vector2 initialOffset = GlobalPosition - startingPosition;
							GlobalPosition = endingPosition + initialOffset;
						}
					}
				}
			}
			if (PumpDetect.HasOverlappingAreas())
			{
				var OverlappingAreas = PumpDetect.GetOverlappingAreas();
				foreach (var area in OverlappingAreas)
				{
					if (area.Name == "Pump_Area")
					{
						Node pumpNode = area.GetParent();
						if (pumpNode != null)
						{
							PumpBehaviour pump = pumpNode as PumpBehaviour;
							pump.IsOpen = !pump.IsOpen;
						}
					}
				}
			}
		}
		bool altInteracted = Input.IsActionJustPressed("interact_alt");
		if (altInteracted)
		{
			//TODO: FIND A USE
		}
	}

	void CameraAreaEntered(Area2D area)
	{
		Globals.CameraCenter = area.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Health = Mathf.Clamp(Health,0,MaxHealth);

		if (isUnderwater)
		{
			OxygenTime -= (float)delta;
			OxygenTime = Mathf.Clamp(OxygenTime,0,MaxOxygenTime);

			if (OxygenTime == 0) {
				Health -= (float)delta*20f;
				Health = Mathf.Clamp(Health,0,MaxHealth);
			}
		} else {
			OxygenTime += (float)delta*3f;
			OxygenTime = Mathf.Clamp(OxygenTime,0,MaxOxygenTime);
		}
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
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}
		}
		else
		{
			if (direction != Vector2.Zero)
			{
				velocity = direction * Speed;
			}
			else
			{
				velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
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
