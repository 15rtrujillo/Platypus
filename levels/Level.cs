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
			0 => Lane1,
			1 => Lane2,
			2 => Lane3,
			3 => Lane4,
			4 => Lane5,
			5 => Lane6,
			6 => Lane7,
			7 => Lane8,
			8 => Lane9,
			9 => Lane10,
			_ => null,
		};
	}
}
