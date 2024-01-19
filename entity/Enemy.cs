using Godot;
using System;

public partial class Enemy : Area2D
{
	public int Speed { get; set; } = 100;
	
	private Color _spriteColor = new Color(Colors.White);
	public Color SpriteColor { 
		get
		{
			return _spriteColor;
		}
		set
		{
			GetNode<Sprite2D>("Sprite2D").Modulate = value;
			_spriteColor = value;
		}
	}
	
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
				GetNode<Sprite2D>("Sprite2D").FlipH = true;
			}
			_direction = value;
		}
	}
	
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}