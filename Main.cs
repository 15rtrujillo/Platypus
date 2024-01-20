using Godot;
using System;

public partial class Main : Node
{
	[ExportGroup("Levels")]
    [Export]
    public int CurrentLevel { get; set; } = 0;
    [Export]
	public Godot.Collections.Array<Level> Levels { get; set; }
	
	private Level _currentLevel;
	private Player _player;

    public override void _Ready()
	{
		_currentLevel = Levels[CurrentLevel];

		_player = GetNode<Player>("Player");
		_player.Position = GetNode<Marker2D>("PlayerSpawnLocation").Position;

		StartLevel();
	}
	
	private void StartLevel()
	{
		foreach (EnemyData enemyData in _currentLevel.Enemies)
		{
			Timer timer = new Timer();
			timer.Name = enemyData.GetEnemyName() + "SpawnTimer";
			timer.WaitTime = enemyData.SpawnInterval;
			EnemyData thisEnemyData = enemyData;
			timer.Connect(Timer.SignalName.Timeout, Callable.From(() => {
                Enemy enemy = thisEnemyData.Scene.Instantiate<Enemy>();
                InitializeEnemy(enemy, thisEnemyData.Speed, thisEnemyData.SpawnLocation);
            }));
			AddChild(timer);
            timer.Start();
        }
	}
	
	private void InitializeEnemy(Enemy enemy, int speed, int spawnLocation)
	{
		enemy.Speed = speed;

		Vector2 direction = Vector2.Right;
		if (spawnLocation == 1 || spawnLocation == 2)
		{
			direction = Vector2.Left;
		}
		enemy.Direction = direction;

		enemy.SpriteColor = new Color((float)GD.RandRange(0.1, 1.0), (float)GD.RandRange(0.1, 1.0), (float)GD.RandRange(0.1, 1.0));

		enemy.Position = GetNode<Marker2D>($"SpawnLocation{spawnLocation}").Position;
		AddChild(enemy);
	}
}
