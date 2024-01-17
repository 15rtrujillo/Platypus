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
	
	private void OnSedanSpawnTimerTimeout()
	{
		Enemy sedan = _sedanScene.Instantiate<Enemy>();
		sedan.Speed = 250;
		sedan.Direction = Vector2.Left;
		sedan.SpriteColor = new Color(GD.Randf(), GD.Randf(), GD.Randf());
		sedan.Position = GetNode<Marker2D>("SedanSpawnLocation").Position;
		AddChild(sedan);
	}
}
