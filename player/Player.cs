using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;
using Platypus.Obstacles.Enemies;
using Platypus.Obstacles.Platforms;

namespace Platypus.PlayerNS;

public partial class Player : Area2D
{
	private enum MovementDirection { Left, Up, Right, Down }

	[ExportGroup("Playfield Boundary")]
	[Export]
	public Vector2 MinBound { get; private set; } = new Vector2(25, 125);
	[Export]
	public Vector2 MaxBound { get; private set; } = new Vector2(1025, 1375);

	public bool CanMove { get; set; } = false;
	public bool InWater { get; set; } = false;

	public delegate void PlayerDiedEventHandler(string how);
	public event PlayerDiedEventHandler PlayerDied;
	public delegate void PlayerMovedForwardEventHandler();
	public event PlayerMovedForwardEventHandler PlayerMovedForward;

	private Sprite2D _sprite;
	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier;
	private Vector2 _newPosition;
	private MovementDirection _movementDirection;
	private List<Platform> _platforms = new();
	private bool _isMoving = false;
	private int _currentForward = 0;
	private int _maxForward = 0;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_visibleOnScreenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

		AreaEntered += OnAreaEntered;
		_visibleOnScreenNotifier.ScreenExited += OnScreenExited;
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
				_movementDirection = MovementDirection.Up;
				Rotation = 0;
			}
			else if (@event.IsActionPressed("move_down"))
			{
				_newPosition += Vector2.Down * spriteY;
				_movementDirection = MovementDirection.Down;
				Rotation = Mathf.Pi;
			}
			else if (@event.IsActionPressed("move_left"))
			{
				_newPosition += Vector2.Left * spriteX;
				_movementDirection = MovementDirection.Left;
				Rotation = 3.0f * Mathf.Pi / 2.0f;
			}
			else if (@event.IsActionPressed("move_right"))
			{
				_newPosition += Vector2.Right * spriteX;
				_movementDirection = MovementDirection.Right;
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
		if (InWater && !isOnPlatform)
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
			if (_movementDirection == MovementDirection.Up)
			{
				if (++_currentForward > _maxForward)
				{
					_maxForward = _currentForward;
					PlayerMovedForward?.Invoke();
				}
			}

			else if (_movementDirection == MovementDirection.Down)
			{
				--_currentForward;
			}

			Position = _newPosition;
			_isMoving = false;
		}
	}

	private bool IsInPlayfield(Vector2 position)
	{
		return position.X > MinBound.X && position.Y > MinBound.Y
				&& position.X < MaxBound.X && position.Y < MaxBound.Y;
	}

	public void Die(string how)
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

	private void OnScreenExited()
	{
		Die("died");
	}
}
