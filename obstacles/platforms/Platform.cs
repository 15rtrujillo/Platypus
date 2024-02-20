using Godot;
using Platypus.PlayerNS;

namespace Platypus.Obstacles.Platforms;

public abstract partial class Platform : Obstacle
{
    protected Player _player;

	public override void _Ready()
	{
		base._Ready();

        AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

        AreaEntered -= OnAreaEntered;
        AreaExited -= OnAreaExited;
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
			player.LeftPlatform(this);
			_player = null;
		}
	}
}
