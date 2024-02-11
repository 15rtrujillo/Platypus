using Godot;
using System.Collections.Generic;

namespace Platypus.Obstacles;

public partial class Turtle : Obstacle
{
	private readonly List<AnimatedSprite2D> _sprites = new();

	public override void _Ready()
	{
		foreach (Node node in GetChildren())
		{
			if (node is AnimatedSprite2D sprite)
			{
				_sprites.Add(sprite);
			}
		}
	}

	protected override void Flip()
	{
		foreach (AnimatedSprite2D sprite in _sprites)
		{
			sprite.FlipH = true;
		}
	}
}
