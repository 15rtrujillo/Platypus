using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public Vector2 MinBound { get; set; } = new Vector2(25, 125);
	[Export]
	public Vector2 MaxBound { get; set; } = new Vector2(1025, 1375);
	
	[Signal]
	public delegate void PlayerDiedEventHandler();

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey)
		{
			Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
			int spriteX = sprite.Texture.GetWidth();
			int spriteY = sprite.Texture.GetHeight();

			Vector2 newPosition = Position;

			if (@event.IsActionPressed("move_up"))
			{
				newPosition -= new Vector2(0, spriteY);
				Rotation = 0;
			}
			else if (@event.IsActionPressed("move_down"))
			{
				newPosition += new Vector2(0, spriteY);
				Rotation = Mathf.Pi;
			}
			else if (@event.IsActionPressed("move_left"))
			{
				newPosition -= new Vector2(spriteX, 0);
				Rotation = (3.0f * Mathf.Pi) / 2.0f;
			}
			else if (@event.IsActionPressed("move_right"))
			{
				newPosition += new Vector2(spriteX, 0);
				Rotation = Mathf.Pi / 2.0f;
			}

			if ((newPosition.X > MinBound.X && newPosition.Y > MinBound.Y)
				&& (newPosition.X < MaxBound.X && newPosition.Y < MaxBound.Y))
			{
				Position = newPosition;
			}
		}
	}
	
	private void OnAreaEntered(Area2D area)
	{
		if (area is Enemy)
		{
			EmitSignal(SignalName.PlayerDied);
			Hide();
		}
	}
}
