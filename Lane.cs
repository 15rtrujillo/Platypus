using Godot;
using Platypus.Obstacles;
using System.Collections.Generic;

public partial class Lane : Node2D
{
    public enum Side
    {
        Right,
        Left
    }

    [Export]
    public PackedScene Obstacle { get; set; }
    [Export]
    public int Speed { get; set; }
    [Export]
    public int SpawnInterval { get; set; }
    [Export]
    public Side SpawnSide { get; set; }

    private readonly LinkedList<Obstacle> _obstacle = new();
    private Timer _spawnTimer;

	public override void _Ready()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Timeout += OnSpawnTimerTimeout;
	}

    private void OnSpawnTimerTimeout()
    {

    }
}
