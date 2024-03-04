using Godot;
using Platypus.Levels;
using Platypus.PlayerNS;
using Platypus.PlayfieldNS;
using Platypus.UserInterface;

namespace Platypus;

public partial class Main : Node
{
	[ExportGroup("Lives")]
	[Export]
	public int Lives { get; private set; } = 3;
	[Export]
	public int OneUp { get; private set; } = 20000;

	[ExportGroup("Scoring")]
	[Export]
	public int EnteredNestScore { get; private set; } = 50;
	[Export]
	public int PerTickRemainingScore { get; private set; }= 10;
	[Export]
	public int MoveForwardScore { get; private set; } = 10;
	[Export]
	public int EndOfLevelScore { get; private set; } = 1000;

	[ExportGroup("Levels")]
	[Export]
	public int CurrentLevel { get; private set; } = 0;
	[Export]
	public Godot.Collections.Array<Level> Levels { get; private set; } = new();

	private Level _level;
	private Timer _levelTimer;
	private Playfield _playfield;
	private Player _player;
	private Marker2D _playerSpawn;
	private GameUI _gameUI;
	private MessageBox _messageBox;
	private PackedScene _mainMenuScene;
	private int _score = 0;
	private int _occupiedNests = 0;
	private int _totalTicks;
	private int _currentTick = 0;
	private bool _oneUpAwarded = false;

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
		_player.PlayerMovedForward += OnPlayerMovedForward;

		_playerSpawn = GetNode<Marker2D>("PlayerSpawnLocation");

		_gameUI = GetNode<GameUI>("MainUI");

		for (int i = 0; i < Lives; ++i)
		{
			_gameUI.OneUp();
		}

		_gameUI.UpdateOneUpLabel(OneUp);

		_messageBox = GetNode<MessageBox>("MessageBox");

		_mainMenuScene = ResourceLoader.Load<PackedScene>("res://ui/MainMenu.tscn");

		StartLevel();
	}

	private async void StartLevel()
	{
		_level = Levels[CurrentLevel];

		_player.Position = _playerSpawn.Position;
		_player.GetNode<Sprite2D>("Sprite2D").Show();

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

		_player.GetNode<Sprite2D>("Sprite2D").Hide();
		_player.CanMove = false;
		_player.Position = _playerSpawn.Position;

		_playfield.StopLevel();
	}

	private async void EndGame(string message)
	{
		await _messageBox.DisplayMessage(message);
		GetTree().ChangeSceneToPacked(_mainMenuScene);
	}

	private async void WinLevel()
	{
		_occupiedNests = 0;
		_playfield.ResetLevel();
		++CurrentLevel;

		IncrementScore((_totalTicks - _currentTick) * PerTickRemainingScore);
		IncrementScore(EndOfLevelScore);

		await _messageBox.DisplayMessage("You got them all home!");

		if (CurrentLevel >= Levels.Count)
		{
			EndGame("Congratulations! You won the game!");
			return;
		}

		StartLevel();
	}

	private void IncrementScore(int howMuch)
	{
		_score += howMuch;
		_gameUI.UpdateScore(_score);
		if (!_oneUpAwarded && _score >= OneUp)
		{
			_gameUI.OneUp();
			_oneUpAwarded = true;
		}
	}

	private void OnPlayerEnteredNest()
	{
		_player.Position = _playerSpawn.Position;

		_currentTick = 0;
		_gameUI.UpdateProgressBar(1.0f);

		++_occupiedNests;

		IncrementScore(EnteredNestScore);

		if (_occupiedNests >= 5)
		{
			StopLevel();
			WinLevel();
		}
	}

	private async void OnPlayerDied(string how)
	{
		StopLevel();

		--Lives;
		_gameUI.RemoveLife();

		await _messageBox.DisplayMessage($"You {how}!");

		if (Lives < 0)
		{
			EndGame("You've lost all your lives. Game over!");
			return;
		}

		StartLevel();
	}

	private void OnPlayerMovedForward()
	{
		IncrementScore(MoveForwardScore);
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
