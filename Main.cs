using Godot;
using Platypus.Entities;
using Platypus.Entities.Enemies;
using Platypus.Levels;
using Platypus.PlayerNS;
using Platypus.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platypus;

public partial class Main : Node
{
	[ExportGroup("Levels")]
	[Export]
	public int CurrentLevel { get; set; } = 0;
	[Export]
	public Godot.Collections.Array<Level> Levels { get; set; }

	private Level _level;
	private Timer _levelTimer;
	private Player _player;
	private Marker2D _playerSpawn;
	private GameUI _gameUI;
	private MessageBox _messageBox;
	private int _score = 0;
	private int _lives = 4;
	private int _totalTicks;
	private int _currentTick = 0;

	private readonly Dictionary<Area2D, Area2D.AreaEnteredEventHandler> _nestEventHandlers = new();
	private readonly Dictionary<Timer, Action> _timers = new();

	public override void _Ready()
	{
		_level = Levels[CurrentLevel];

		_levelTimer = GetNode<Timer>("LevelTimer");
		_levelTimer.Timeout += OnLevelTimerTimeout;

		_player = GetNode<Player>("Player");
		_player.PlayerDied += OnPlayerDied;

		_playerSpawn = GetNode<Marker2D>("PlayerSpawnLocation");

		_gameUI = GetNode<GameUI>("MainUI");

		_messageBox = GetNode<MessageBox>("MessageBox");

		StartLevel();
	}

	private async void StartLevel()
	{
		_player.Position = _playerSpawn.Position;
		_player.Show();

		_totalTicks = _level.TimeLimit * 2;

		SetupNestEventHandlers();
		SetupEnemyTimers();

		await _messageBox.DisplayMessage("Get ready!");

		_player.CanMove = true;
		_levelTimer.Start();
	}

	private void SetupNestEventHandlers()
	{
		foreach (Node child in GetNode<Node2D>("Playfield").GetChildren())
		{
			if (child is Area2D)
			{
				Area2D areaNode = (Area2D)child;

				// Make sure we don't add duplicate handlers if we're restarting the level
				if (_nestEventHandlers.Keys.Contains(areaNode)) continue;

				void nestEnteredHandler(Area2D hitBy)
				{
					OnNestEntered(hitBy, areaNode);
				}

				_nestEventHandlers.Add(areaNode, nestEnteredHandler);

				areaNode.AreaEntered += nestEnteredHandler;
			}
		}
	}

	private void SetupEnemyTimers()
	{
		foreach (EntityData enemyData in _level.Enemies)
		{
			EntityData thisEnemyData = enemyData;

			Timer timer = new()
			{
				Name = thisEnemyData.GetEntityName() + "SpawnTimer",
				WaitTime = thisEnemyData.SpawnInterval
			};

			void timerAction()
			{
				Entity enemy = thisEnemyData.Scene.Instantiate<Entity>();
				InitializeEnemy(enemy, thisEnemyData);
			}

			timer.Timeout += timerAction;

			AddChild(timer);
			_timers.Add(timer, timerAction);

			timer.Start();
		}
	}

	private void InitializeEnemy(Entity enemy, EntityData enemyData)
	{
		AddChild(enemy);
		enemy.Speed = enemyData.Speed;

		enemy.Direction = (enemyData.SpawnLocation == 1 || enemyData.SpawnLocation == 2) ? Vector2.Left : Vector2.Right;

		if (enemy is Car car)
		{
			car.SpriteColor = new((float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0));
		}

		enemy.Position = GetNode<Marker2D>($"SpawnLocation{enemyData.SpawnLocation}").Position;
	}

	private void OnNestEntered(Area2D area, Area2D whichNest)
	{
		// Probably increment some score

		if (area is Player player)
		{
			player.Position = GetNode<Marker2D>("PlayerSpawnLocation").Position;
			Sprite2D sprite = new()
			{
				Texture = ResourceLoader.Load<Texture2D>("res://entity/platypus.png")
			};
			whichNest.AddChild(sprite);

			whichNest.AreaEntered -= _nestEventHandlers[whichNest];
			_nestEventHandlers.Remove(whichNest);
		}
	}

	private async void OnPlayerDied(string how)
	{
		_player.Hide();
		_player.CanMove = false;
		_player.Position = _playerSpawn.Position;
		--_lives;
		_gameUI.RemoveLife();

		foreach ((Timer timer, Action action) in _timers)
		{
			timer.Timeout -= action;
			timer.Stop();
			_timers.Remove(timer);
			timer.QueueFree();
		}

		await _messageBox.DisplayMessage($"You {how}!");
		StartLevel();
	}

	private void OnLevelTimerTimeout()
	{
		_gameUI.UpdateProgressBar(1.0f - (++_currentTick / (float)_totalTicks));

		if (_currentTick >= _totalTicks)
		{
			OnPlayerDied("just ran out of time");
		}
	}
}
