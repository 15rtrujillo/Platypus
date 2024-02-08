using System;
using Godot;

namespace Platypus.Obstacles.Enemies;

public partial class Car : Obstacle
{
	[Export]
	public string CarName { get; set; }

	private Color _spriteColor = new(Colors.White);
	public Color SpriteColor
	{
		get
		{
			return _spriteColor;
		}
		set
		{
			if (_sprite is null)
			{
				throw new NullReferenceException("Attempting to set car color before car has been added to the tree");
			}
			_sprite.Modulate = value;
			_spriteColor = value;
		}
	}

	private Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		base._Ready();
	}

	protected override void Flip()
	{
		_sprite.FlipH = true;
	}
}
