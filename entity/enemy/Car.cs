using System.Transactions;
using Godot;

namespace Platypus.Entity
{
	public partial class Car : Enemy
	{
		[Export]
		public string CarName { get; set; }

		private Color _spriteColor = new Color(Colors.White);
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
