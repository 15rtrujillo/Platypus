using System.Transactions;
using Godot;

namespace Platypus.Entities.Enemies
{
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
	}
}
