using Godot;
using Platypus.PlayfieldNS;
using System;

namespace Platypus.Levels;

[GlobalClass]
public partial class Level : Resource
{
	[Export]
	public int TimeLimit { get; private set; } = 30;

	[Export]
	public LaneData Lane1 { get; private set; }
	[Export]
	public LaneData Lane2 { get; private set; }
	[Export]
	public LaneData Lane3 { get; private set; }
	[Export]
	public LaneData Lane4 { get; private set; }
	[Export]
	public LaneData Lane5 { get; private set; }
	[Export]
	public LaneData Lane6 { get; private set; }
	[Export]
	public LaneData Lane7 { get; private set; }
	[Export]
	public LaneData Lane8 { get; private set; }
	[Export]
	public LaneData Lane9 { get; private set; }
	[Export]
	public LaneData Lane10 { get; private set; }

	public LaneData GetLaneData(int index)
	{
		return index switch
		{
			1 => Lane1,
			2 => Lane2,
			3 => Lane3,
			4 => Lane4,
			5 => Lane5,
			6 => Lane6,
			7 => Lane7,
			8 => Lane8,
			9 => Lane9,
			10 => Lane10,
			_ => null,
		};
	}
}
