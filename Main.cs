using Godot;
using System;

public partial class Main : Node
{
	private Player _player;
	private PackedScene _sedanScene;
	private PackedScene _semi_TruckScene;
	
	public override void _Ready()
	{
		_player = GetNode<Player>("Player");
		_player.Position = GetNode<Marker2D>("SpawnLocation").Position;
		
		_sedanScene = ResourceLoader.Load<PackedScene>("res://entity/Sedan.tscn");
		_semi_TruckScene = ResourceLoader.Load<PackedScene>("res://entity/Semi_Truck.tscn");
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
		InitializeEnemy(sedan, 300, Vector2.Left);
	}
	
	private void OnSemi_TruckSpawnTimerTimeout()
	{
		Enemy semi_Truck = _semi_TruckScene.Instantiate<Enemy>();
		InitializeEnemy(semi_Truck, 150, Vector2.Right);
	}
}
