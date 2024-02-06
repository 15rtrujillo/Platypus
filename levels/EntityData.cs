using Godot;
using System.IO;

namespace Platypus.Levels
{
	[GlobalClass]
	public partial class EntityData : Resource
	{
		[Export]
		public PackedScene Scene { get; set; }
		[Export]
		public int Speed { get; set; }

		[ExportGroup("Spawning")]
		[Export(PropertyHint.Range, "0,4,")]
		public int SpawnLocation { get; set; }
		[Export]
		public float SpawnInterval { get; set; }

		public string GetEntityName()
		{
			return Path.GetFileNameWithoutExtension(Scene.ResourcePath);
		}
	}
}
