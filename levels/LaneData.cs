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
	public PackedScene Obstacle { get; set; }
	[Export]
	public int Speed { get; set; }

	[ExportGroup("Spawning")]
	[Export]
	public Side SpawnFrom { get; set; }
	[Export]
	public float SpawnInterval { get; set; }

	public string GetObstacleName()
	{
		return Path.GetFileNameWithoutExtension(Obstacle.ResourcePath);
	}
}
