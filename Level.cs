using Godot;
using System;

[GlobalClass]
public partial class Level : Resource
{
	[Export]
	public int LevelTime { get; set; }
	[ExportGroup("Sedan")]
	[Export]
	public int SedanSpeed { get; set; }
	[Export]
	public float SedanSpawnInterval { get; set; }
	[ExportGroup("Semi-Truck")]
	[Export]
	public int SemiTruckSpeed { get; set; }
	[Export]
	public float SemiTruckSpawnInterval { get; set; }
}
