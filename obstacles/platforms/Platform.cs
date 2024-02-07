using Godot;
using System.Collections.Generic;

namespace Platypus.Obstacles;

public partial class Platform : Obstacle
{
	private readonly List<Sprite2D> _sprites = new();

	public override void _Ready()
	{
		base._Ready();

		foreach (Node child in GetChildren())
		{
			if (child is Sprite2D sprite)
			{
				_sprites.Add(sprite);
			}
		}
	}

	protected override void Flip()
	{
		foreach (Sprite2D sprite in _sprites)
		{
			sprite.FlipH = true;
		}
	}
}
