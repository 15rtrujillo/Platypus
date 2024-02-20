using Godot;
using Platypus.Levels;
using System.Collections.Generic;
using System.Linq;

namespace Platypus.PlayfieldNS;

public partial class Playfield : Node2D
{
	public List<Nest> Nests { get; } = new();
	public List<Lane> Lanes { get; } = new();

	public override void _Ready()
	{
		foreach (Nest nest in GetNode<Node2D>("Nests").GetChildren().Cast<Nest>())
		{
			Nests.Add(nest);
		}

		foreach (Lane lane in GetNode<Node2D>("Lanes").GetChildren().Cast<Lane>())
		{
			Lanes.Add(lane);
		}
	}

	public void StartLevel(Level level)
	{
		for (int i = 0; i < Lanes.Count; ++i)
		{
			Lanes[i].ConfigureLane(level[i]);
			Lanes[i].Start();
		}
	}

	public void StopLevel()
	{
		foreach (Lane lane in Lanes)
		{
			lane.Stop();
		}
	}

	public void ResetLevel()
	{
		foreach (Nest nest in Nests)
		{
			nest.Reset();
		}
	}
}
