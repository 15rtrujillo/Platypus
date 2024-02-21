using Godot;
using Platypus.Levels;
using Platypus.PlayerNS;
using Platypus.PlayfieldNS;
using Platypus.UserInterface;

namespace Platypus;

public partial class Main : Node
{
	[ExportGroup("Levels")]
	[Export]
	public int CurrentLevel { get; set; } = 0;
	[Export]
	public Godot.Collections.Array<Level> Levels { get; private set; } = new();

	private Level _level;
	private Timer _levelTimer;
	private Playfield _playfield;
	private Player _player;
	private Marker2D _playerSpawn;
	private GameUI _gameUI;
	private MessageBox _messageBox;
	private int _score = 0;
	private int _occupiedNests = 0;
	private int _lives = 4;
	private int _totalTicks;
	private int _currentTick = 0;

	public override void _Ready()
	{
		_levelTimer = GetNode<Timer>("LevelTimer");
		_levelTimer.Timeout += OnLevelTimerTimeout;

		_playfield = GetNode<Playfield>("Playfield");
		foreach (Nest nest in _playfield.Nests)
		{
			nest.PlayerEnteredNest += OnPlayerEnteredNest;
		}

		_player = GetNode<Player>("Player");
		_player.PlayerDied += OnPlayerDied;

		_playerSpawn = GetNode<Marker2D>("PlayerSpawnLocation");

		_gameUI = GetNode<GameUI>("MainUI");

		_messageBox = GetNode<MessageBox>("MessageBox");

		StartLevel();
	}

	private async void StartLevel()
	{
		_level = Levels[CurrentLevel];

		_player.Position = _playerSpawn.Position;
		_player.Show();

		_currentTick = 0;
		_totalTicks = _level.TimeLimit * 2;

		_playfield.StartLevel(_level);

		await _messageBox.DisplayMessage("Get ready!");

		_player.CanMove = true;
		_levelTimer.Start();
	}

	private void StopLevel()
	{
		_levelTimer.Stop();

		_player.Hide();
		_player.CanMove = false;
		_player.Position = _playerSpawn.Position;

		_playfield.StopLevel();
	}

	private void OnPlayerEnteredNest()
	{
		_player.Position = GetNode<Marker2D>("PlayerSpawnLocation").Position;

		_currentTick = 0;
		_gameUI.UpdateProgressBar(1.0f);

		++_occupiedNests;

		// TODO: Update score

		if (_occupiedNests >= 5)
		{
			StopLevel();
			WinLevel();
		}
	}

	private void WinLevel()
	{
		_occupiedNests = 0;
		_playfield.ResetLevel();
		++CurrentLevel;

		// TODO: Update score based on remaining time

		StartLevel();
	}

	private async void OnPlayerDied(string how)
	{
		StopLevel();

		--_lives;
		_gameUI.RemoveLife();

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
