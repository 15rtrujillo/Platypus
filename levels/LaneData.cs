using Godot;
using System.IO;

namespace Platypus.Levels;

[GlobalClass]
public partial class LaneData : Resource
{
	public enum Side
	{
		Right,
		Left
	}

	[Export]
	public PackedScene Obstacle { get; private set; }
	[Export]
	public int Speed { get; private set; } = 100;
	[Export]
	public Side SpawnFrom { get; private set; }
	[Export]
	public float SpawnInterval { get; private set; }

	public string GetObstacleName()
	{
		return Path.GetFileNameWithoutExtension(Obstacle.ResourcePath);
	}
}
