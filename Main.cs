using Godot;
using System;

public partial class Main : Node
{
	[ExportGroup("Levels")]
    // Debug option for starting at a certain level. The levels are zero-indexed.
    [Export]
    public int BeginAtLevel { get; set; } = 0;
    [Export]
	public Godot.Collections.Array<Level> Levels { get; set; }
	
	private int _currentLevel;
	private Player _player;
	private PackedScene _sedanScene;
	private PackedScene _semi_TruckScene;
    private PackedScene _coupeScene;

    public override void _Ready()
	{
		_player = GetNode<Player>("Player");
		_player.Position = GetNode<Marker2D>("SpawnLocation").Position;
		
		_sedanScene = ResourceLoader.Load<PackedScene>("res://entity/Sedan.tscn");
		_semi_TruckScene = ResourceLoader.Load<PackedScene>("res://entity/Semi_Truck.tscn");
		_coupeScene = ResourceLoader.Load<PackedScene>("res://entity/Coupe.tscn");

		StartLevel();
	}
	
	private void StartLevel()
	{
		Timer truckSpawnTimer = GetNode<Timer>("Semi_TruckSpawnTimer");
		truckSpawnTimer.WaitTime = Levels[_currentLevel].SemiTruckSpawnInterval;
		truckSpawnTimer.Start();

		Timer sedanSpawnTimer = GetNode<Timer>("SedanSpawnTimer");
		sedanSpawnTimer.WaitTime = Levels[_currentLevel].SedanSpawnInterval;
        sedanSpawnTimer.Start();

		Timer coupeSpawnTimer = GetNode<Timer>("CoupeSpawnTimer");
		coupeSpawnTimer.WaitTime = Levels[_currentLevel].CoupeSpawnInterval;
		coupeSpawnTimer.Start();
	}
	
	private void InitializeEnemy(Enemy enemy, int speed, Vector2 direction)
	{
		enemy.Speed = speed;
		enemy.Direction = direction;
		enemy.SpriteColor = new Color((float)GD.RandRange(0.1, 1.0), (float)GD.RandRange(0.1, 1.0), (float)GD.RandRange(0.1, 1.0));
		enemy.Position = GetNode<Marker2D>($"{enemy.Name}SpawnLocation").Position;
		AddChild(enemy);
	}
	
	private void OnSedanSpawnTimerTimeout()
	{
		Enemy sedan = _sedanScene.Instantiate<Enemy>();
		InitializeEnemy(sedan, Levels[_currentLevel].SedanSpeed, Vector2.Left);
	}
	
	private void OnSemi_TruckSpawnTimerTimeout()
	{
		Enemy semi_Truck = _semi_TruckScene.Instantiate<Enemy>();
		InitializeEnemy(semi_Truck, Levels[_currentLevel].SemiTruckSpeed, Vector2.Right);
	}

	private void OnCoupeSpawnTimerTimeout()
	{
		Enemy coupe = _coupeScene.Instantiate<Enemy>();
		InitializeEnemy(coupe, Levels[_currentLevel].CoupeSpeed, Vector2.Right);
	}
}
