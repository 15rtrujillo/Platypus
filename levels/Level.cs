using Godot;
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

	public LaneData this[int index]
	{
		get
		{
			try
			{
				return (LaneData)GetType().GetProperty($"Lane{index + 1}").GetValue(this, null);
			}

			catch (ArgumentException)
			{
				return null;
			}
		}
	}
}
