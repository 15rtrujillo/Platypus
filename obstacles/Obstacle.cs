using Godot;
using System;

namespace Platypus.Obstacles;

public abstract partial class Obstacle : Area2D
{
	public int Speed { get; set; }
	private Vector2 _direction = Vector2.Left;
	public Vector2 Direction
	{
		get
		{
			return _direction;
		}
		set
		{
			if (value == Vector2.Right)
			{
				Flip();
			}
			_direction = value;
		}
	}

	private VisibleOnScreenNotifier2D _onScreenNotifier;

	public override void _Ready()
	{
		_onScreenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

		if (_onScreenNotifier is null)
		{
			throw new NullReferenceException("Obstacles must contain a VisibleOnScreenNotifier2D node of the same name.");
		}

		_onScreenNotifier.ScreenExited += OnVisibleOnScreenNotifier2DScreenExited;
	}

	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	public override void _ExitTree()
	{
		_onScreenNotifier.ScreenExited -= OnVisibleOnScreenNotifier2DScreenExited;
	}

	protected abstract void Flip();

	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
