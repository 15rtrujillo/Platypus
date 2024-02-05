using Godot;
using System;

namespace PlatypusGame.Levels
{
	[GlobalClass]
	public partial class Level : Resource
	{
		[Export]
		public int TimeLimit { get; set; }

		[Export]
		public Godot.Collections.Array<EnemyData> Enemies { get; set; }
	}
}
