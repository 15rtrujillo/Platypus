using System.Collections.Generic;
using Godot;
using Platypus.Obstacles.Enemies;
using Platypus.Obstacles.Platforms;

namespace Platypus.PlayerNS;

public partial class Player : Area2D
{
	[ExportGroup("Playfield Boundary")]
	[Export]
	public Vector2 MinBound { get; private set; } = new Vector2(25, 125);
	[Export]
	public Vector2 MaxBound { get; private set; } = new Vector2(1025, 1375);

	[ExportGroup("Water Boundary")]
	[Export]
	public Vector2 MinWaterBound { get; private set; } = new Vector2(0, 200);
	[Export]
	public Vector2 MaxWaterBound { get; private set; } = new Vector2(1050, 700);

	public bool CanMove { get; set; } = false;

	public delegate void PlayerDiedEventHandler(string how);
	public event PlayerDiedEventHandler PlayerDied;

	private Sprite2D _sprite;
	private Vector2 _newPosition;
	private List<Platform> _platforms = new();
	private bool _isMoving = false;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");

		AreaEntered += OnAreaEntered;
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event is InputEventKey)
		{
			if (!CanMove) return;

			int spriteX = _sprite.Texture.GetWidth();
			int spriteY = _sprite.Texture.GetHeight();

			_newPosition = Position;

			if (@event.IsActionPressed("move_up"))
			{
				_newPosition += Vector2.Up * spriteY;
				Rotation = 0;
			}
			else if (@event.IsActionPressed("move_down"))
			{
				_newPosition += Vector2.Down * spriteY;
				Rotation = Mathf.Pi;
			}
			else if (@event.IsActionPressed("move_left"))
			{
				_newPosition += Vector2.Left * spriteX;
				Rotation = 3.0f * Mathf.Pi / 2.0f;
			}
			else if (@event.IsActionPressed("move_right"))
			{
				_newPosition += Vector2.Right * spriteX;
				Rotation = Mathf.Pi / 2.0f;
			}

			if (_newPosition != Position)
			{
				_isMoving = true;
				GetViewport().SetInputAsHandled();
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Check for drowning
		bool isOnPlatform = _platforms.Count > 0;
		if (IsInWater(Position) && !isOnPlatform)
		{
			Die("drowned");
		}

		if (_platforms.Count > 0)
		{
			Platform platform = _platforms[0];
			Position += platform.Speed * platform.Direction * (float)delta;
		}

		if (_isMoving && IsInPlayfield(_newPosition))
		{
			Position = _newPosition;
			_isMoving = false;
		}
	}

	private bool IsInPlayfield(Vector2 position)
	{
		return position.X > MinBound.X && position.Y > MinBound.Y
				&& position.X < MaxBound.X && position.Y < MaxBound.Y;
	}

	private bool IsInWater(Vector2 position)
	{
		return position.X > MinWaterBound.X && position.Y > MinWaterBound.Y
				&& position.X < MaxWaterBound.X && position.Y < MaxWaterBound.Y;

	}

	private void Die(string how)
	{
		PlayerDied?.Invoke(how);
	}

	public void LandedOnPlatform(Platform paltform)
	{
		_platforms.Add(paltform);
	}

	public void LeftPlatform(Platform platform)
	{
		_platforms.Remove(platform);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Car car)
		{
			Die($"got ran over by a {car.CarName.ToLower()}");
		}
	}
}
