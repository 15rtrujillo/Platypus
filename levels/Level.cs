using Godot;

namespace Platypus.Levels
{
	[GlobalClass]
	public partial class Level : Resource
	{
		[Export]
		public int TimeLimit { get; set; }

		[Export]
		public Godot.Collections.Array<EntityData> Enemies { get; set; }
	}
}
