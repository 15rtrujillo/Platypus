using Godot;
using Platypus.PlayerNS;

namespace Platypus.PlayfieldNS;

public partial class Nest : Area2D
{
	private bool _occupied = false;
	private Sprite2D _sprite;

	public delegate void PlayerEnteredNestEventHandler();
	public event PlayerEnteredNestEventHandler PlayerEnteredNest;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");

		AreaEntered += OnNestEntered;
	}

	public void Reset()
	{
		_occupied = false;
		_sprite.Hide();
	}

	private void OnNestEntered(Area2D area)
	{
		if (!_occupied && area is Player)
		{
			_occupied = true;
			_sprite.Show();
			PlayerEnteredNest?.Invoke();
		}
	}
}
