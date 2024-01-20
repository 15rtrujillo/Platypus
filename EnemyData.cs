using Godot;
using System;

[GlobalClass]
public partial class EnemyData : Resource
{
    [Export]
    public PackedScene Scene { get; set; }
    [Export]
    public int Speed { get; set; }

    [ExportGroup("Spawning")]
    [Export(PropertyHint.Range, "0-4")]
    public int SpawnLocation { get; set; }
    [Export]
    public float SpawnInterval { get; set; }

    public string GetEnemyName()
    {
        int firstLetterIndex = Scene.ResourcePath.LastIndexOf("/") + 1;
        int dotIndex = Scene.ResourcePath.IndexOf(".");
        return Scene.ResourcePath.Substring(firstLetterIndex, dotIndex - firstLetterIndex);
    }
}
