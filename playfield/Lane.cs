using Godot;
using Platypus.Levels;
using Platypus.Obstacles;
using Platypus.Obstacles.Enemies;
using System;
using System.Collections.Generic;

namespace Platypus.PlayfieldNS;

public partial class Lane : Node2D
{
	private LaneData _data;
	private Timer _spawnTimer;
	private Marker2D _leftSpawnLocation;
	private Marker2D _rightSpawnLocation;
	private List<Obstacle> _obstacles = new();

	public override void _Ready()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnTimer.Timeout += OnSpawnTimerTimeout;

		_leftSpawnLocation = GetNode<Marker2D>("SpawnLocationLeft");
		_rightSpawnLocation = GetNode<Marker2D>("SpawnLocationRight");
	}

	public void ConfigureLane(LaneData laneData)
	{
		if (laneData is null || laneData == _data)
		{
			return;
		}

		_data = laneData;

		if (_spawnTimer is null)
		{
			throw new NullReferenceException("Attemmpting to attach data before lane is ready. Make sure to add the Lane to the tree before calling this method.");
		}

		_spawnTimer.WaitTime = _data.SpawnInterval;
	}

	public void Start()
	{
		if (_data is null)
		{
			return;
		}

		// Spawn an obstacle
		OnSpawnTimerTimeout();

		_spawnTimer.Start();
	}

	public void Stop()
	{
		_spawnTimer.Stop();
		foreach (Obstacle obstacle in _obstacles)
		{
			if (IsInstanceValid(obstacle))
			{
				obstacle.QueueFree();
			}
		}

		_obstacles.Clear();
	}

	private void OnSpawnTimerTimeout()
	{
		Obstacle obstacle = _data.Obstacle.Instantiate<Obstacle>();

		_obstacles.Add(obstacle);
		AddChild(obstacle);

		obstacle.Speed = _data.Speed;

		CollisionShape2D collider = obstacle.GetNode<CollisionShape2D>("CollisionShape2D");
		float width = ((RectangleShape2D)collider.Shape).Size.X;
		if (_data.SpawnFrom == LaneData.Side.Right)
		{
			obstacle.Direction = Vector2.Left;
			obstacle.Position = _rightSpawnLocation.Position + Vector2.Right * (width / 2);
		}

		else
		{
			obstacle.Direction = Vector2.Right;
			obstacle.Position = _leftSpawnLocation.Position + Vector2.Left * (width / 2);
		}

		if (obstacle is Car car)
		{
			car.SpriteColor = new((float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0));
		}
	}
}
