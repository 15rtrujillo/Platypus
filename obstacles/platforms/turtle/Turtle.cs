using Godot;
using Platypus.PlayerNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platypus.Obstacles;

public partial class Turtle : Obstacle
{
	public int SinkMillisecondsDelay { get; set; } = 4000;
	private bool _running = true;
	private readonly List<AnimatedSprite2D> _sprites = new();
	private Player _player;

	public override void _Ready()
	{
		foreach (Node node in GetChildren())
		{
			if (node is AnimatedSprite2D sprite)
			{
				_sprites.Add(sprite);
			}
		}

		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		Sink();
	}

	public override void _ExitTree()
	{
		_running = false;
	}

	protected override void Flip()
	{
		foreach (AnimatedSprite2D sprite in _sprites)
		{
			sprite.FlipH = true;
		}
	}

	private async void Sink()
	{
		await Task.Delay(GD.RandRange(0, SinkMillisecondsDelay));
		while (_running)
		{
			foreach (AnimatedSprite2D sprite in _sprites)
			{
				sprite?.Play("default", Speed / 100.0f);
			}
			Monitoring = false;
			_player?.LeftPlatform();

			await Task.Delay(SinkMillisecondsDelay);
			Monitoring = true;

			foreach (AnimatedSprite2D sprite in _sprites)
			{
				sprite?.Play("default", -1 * (Speed / 100.0f), true);
			}

			await Task.Delay(SinkMillisecondsDelay);
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Player player)
		{
			_player = player;
			_player.LandedOnPlatform(this);
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area is Player player)
		{
			player.LeftPlatform();
			_player = null;
		}
	}
}
