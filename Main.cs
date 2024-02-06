using Godot;
using Platypus.Entity;
using Platypus.Levels;
using Platypus.UserInterface;
using System;
using System.Collections.Generic;

namespace Platypus
{
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
		private GameUI _mainUI;
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

			_mainUI = GetNode<GameUI>("MainUI");

			StartLevel();
		}

		private void StartLevel()
		{
			_player.Position = _playerSpawn.Position;
			_player.Show();

			_totalTicks = _level.TimeLimit * 2;

			SetupNestEventHandlers();
			SetupEnemyTimers();

			_levelTimer.Start();
		}

		private void SetupNestEventHandlers()
		{
			foreach (Node child in GetNode<Node2D>("Playfield").GetChildren())
			{
				if (child is Area2D)
				{
					Area2D areaNode = (Area2D)child;

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
			foreach (EnemyData enemyData in _level.Enemies)
			{
				EnemyData thisEnemyData = enemyData;

				Timer timer = new()
				{
					Name = thisEnemyData.GetEnemyName() + "SpawnTimer",
					WaitTime = thisEnemyData.SpawnInterval
				};

				void timerAction()
				{
					Enemy enemy = thisEnemyData.Scene.Instantiate<Enemy>();
					InitializeEnemy(enemy, thisEnemyData.Speed, thisEnemyData.SpawnLocation);
				}

				timer.Timeout += timerAction;

				AddChild(timer);
				_timers.Add(timer, timerAction);

				timer.Start();
			}
		}

		private void InitializeEnemy(Enemy enemy, int speed, int spawnLocation)
		{
			enemy.Speed = speed;

			enemy.Direction = (spawnLocation == 1 || spawnLocation == 2) ? Vector2.Left : Vector2.Right;

			enemy.SpriteColor = new((float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0), (float)GD.RandRange(0.2, 1.0));

			enemy.Position = GetNode<Marker2D>($"SpawnLocation{spawnLocation}").Position;
			AddChild(enemy);
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

		private void OnPlayerDied()
		{
			_player.Position = _playerSpawn.Position;
			--_lives;
			_mainUI.RemoveLife();

			foreach ((Timer timer, Action action) in _timers)
			{
				timer.Timeout -= action;
				timer.Stop();
				_timers.Remove(timer);
				timer.QueueFree();
			}
		}

		private void OnLevelTimerTimeout()
		{
			_mainUI.UpdateProgressBar(1.0f - (++_currentTick / (float)_totalTicks));
		}
	}
}
