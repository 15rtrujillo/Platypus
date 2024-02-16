using Godot;
using Platypus.Levels;
using Platypus.Obstacles;
using Platypus.Obstacles.Enemies;
using System;

namespace Platypus.PlayfieldNS;

public partial class Lane : Node2D
{
	private LaneData _data;
	private Timer _spawnTimer;
	private Marker2D _leftSpawnLocation;
	private Marker2D _rightSpawnLocation;

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

		_spawnTimer.Start();
	}

	public void Stop()
	{
		_spawnTimer.Stop();
	}

	private void OnSpawnTimerTimeout()
	{
		Obstacle obstacle = _data.Obstacle.Instantiate<Obstacle>();

		AddChild(obstacle);

		obstacle.Speed = _data.Speed;
		if (_data.SpawnFrom == LaneData.Side.Right)
		{
			obstacle.Direction = Vector2.Left;
			obstacle.Position = _rightSpawnLocation.Position;
		}

		else
		{
			obstacle.Direction = Vector2.Right;
			obstacle.Position = _leftSpawnLocation.Position;
		}

		if (obstacle is Car car)
		{
			car.SpriteColor = new((float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0));
		}
	}
}
