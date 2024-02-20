using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platypus.Obstacles.Platforms;

public partial class Turtle : Platform
{
	[Export]
	public bool ShouldSink { get; set; } = true;
	public int SinkMillisecondsDelay { get; set; } = 4000;
	private bool _running = true;
	private readonly List<AnimatedSprite2D> _sprites = new();
	

	public override void _Ready()
	{
		base._Ready();

		foreach (Node node in GetChildren())
		{
			if (node is AnimatedSprite2D sprite)
			{
				_sprites.Add(sprite);
			}
		}

		if (ShouldSink)
		{
			Sink();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
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
			foreach (AnimatedSprite2D sprite in _sprites)
			{
				await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
			}
			Monitoring = false;
			_player?.LeftPlatform(this);

			await Task.Delay(1000);

			if (!_running)
			{
				break;
			}

			Monitoring = true;
			foreach (AnimatedSprite2D sprite in _sprites)
			{
				sprite?.Play("default", -1 * (Speed / 100.0f), true);
			}
			foreach (AnimatedSprite2D sprite in _sprites)
			{
				await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
			}

			await Task.Delay(SinkMillisecondsDelay);
		}
	}
}
