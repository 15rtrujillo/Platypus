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

	private System.Collections.Generic.Dictionary<Area2D, Area2D.AreaEnteredEventHandler> _nestEventHandlers;

	public override void _Ready()
	{
		_currentLevel = Levels[CurrentLevel];

		_player = GetNode<Player>("Player");
		_player.Position = GetNode<Marker2D>("PlayerSpawnLocation").Position;


		_nestEventHandlers = new System.Collections.Generic.Dictionary<Area2D, Area2D.AreaEnteredEventHandler>();

		foreach (Node child in GetNode<Node2D>("Playfield").GetChildren())
		{
			if (child is Area2D)
			{
				Area2D areaNode = (Area2D)child;
				void nestEnteredHandler(Area2D hitBy) => OnNestEntered(hitBy, areaNode);

				_nestEventHandlers.Add(areaNode, nestEnteredHandler);

				areaNode.AreaEntered += nestEnteredHandler;
			}
		}


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

		enemy.SpriteColor = new Color((float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0));

		enemy.Position = GetNode<Marker2D>($"SpawnLocation{spawnLocation}").Position;
		AddChild(enemy);
	}

	private void OnNestEntered(Area2D area, Area2D whichNest)
	{
		// Probably increment some score

		if (area is Player player)
		{
			player.Position = GetNode<Marker2D>("PlayerSpawnLocation").Position;
			Sprite2D sprite = new Sprite2D();
			sprite.Texture = ResourceLoader.Load<Texture2D>("res://entity/platypus.png");
			whichNest.AddChild(sprite);

			whichNest.AreaEntered -= _nestEventHandlers[whichNest];
		}
	}
}
