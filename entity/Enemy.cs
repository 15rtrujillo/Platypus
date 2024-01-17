using Godot;
using System;

public partial class Enemy : Area2D
{
	private Color _spriteColor = new Color(Colors.White);
	
	[Export]
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
	
	[Export]
	public int Speed { get; set; } = 100;
	
	public Vector2 Direction { get; set; } = Vector2.Left;
	
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
