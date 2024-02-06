using Godot;
using System;
using System.Collections.Generic;

namespace Platypus.UserInterface
{
	public partial class GameUI : Control
	{
		private Label _oneUpLabel;
		private Label _scoreLabel;
		private HBoxContainer _livesOrderer;
		private readonly Stack<TextureRect> _lives = new Stack<TextureRect>();
		private ProgressBar _progressBar;
		private PackedScene _lifeTemplate;
		
		public override void _Ready()
		{
			_oneUpLabel = GetNode<Label>("TopContainer/OneUpLabel");
			_scoreLabel = GetNode<Label>("TopContainer/ScoreLabel");
			_livesOrderer = GetNode<HBoxContainer>("BottomContainer/LivesOrderer");
			_progressBar = GetNode<ProgressBar>("BottomContainer/TimerContainer/ProgressBar");
			_lifeTemplate = ResourceLoader.Load<PackedScene>("res://ui/Life.tscn");

			foreach (Node child in _livesOrderer.GetChildren())
			{
				if (child is TextureRect textureRect)
				{
					_lives.Push(textureRect);
				}
			}
		}
		
		public void UpdateProgressBar(float progress)
		{
			_progressBar.Value = progress;
		}

		public void OneUp()
		{
			TextureRect life = _lifeTemplate.Instantiate<TextureRect>();
			life.Name = $"Life{_lives.Count}";
			_livesOrderer.AddChild(life);
			_lives.Push(life);
		}

		public void RemoveLife()
		{
			TextureRect life = _lives.Pop();
			life.QueueFree();
		}
	}
}
