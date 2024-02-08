using Godot;
using Platypus.Levels;
using System;

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
        _data = laneData;

        if (_spawnTimer is null)
        {
            throw new NullReferenceException("Attemmpting to attach data before lane is ready. Make sure to add the Lane to the tree before calling this method.");
        }

        _spawnTimer.WaitTime = _data.SpawnInterval;
    }

    public void Start()
    {
        _spawnTimer.Start();
    }

    public void Stop()
    {
        _spawnTimer.Stop();
    }

    private void OnSpawnTimerTimeout()
    {

    }
}
