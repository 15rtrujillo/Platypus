using Godot;

namespace Platypus.Entities.Enemies;

public partial class Car : Entity
{
	[Export]
	public string CarName { get; set; }

	private Color _spriteColor = new(Colors.White);
	public Color SpriteColor
	{
		get
		{
			return _spriteColor;
		}
		set
		{
			_sprite.Modulate = value;
			_spriteColor = value;
		}
	}

	private Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		base._Ready();
	}

	protected override void Flip()
	{
		_sprite.FlipH = true;
	}
}
