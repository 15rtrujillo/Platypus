using Godot;
using System;

public partial class Main : Node
{
    public override void _Ready()
    {
        Player player = GetNode<Player>("Player");
        player.Position = GetNode<Marker2D>("SpawnLocation").Position;
    }
}
