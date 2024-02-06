using Godot;
using System.Text;

namespace Platypus.UserInterface
{
	public partial class MainMenu : Control
	{
		private Button _startButton;
		private Button _licensesButton;
		private Control _licensesControl;
		private Button _closeButton;
		private RichTextLabel _licenseText;
		private readonly string _fontLicensePath = "res://fonts/PressStart2P/LICENSE-65f9.txt";
		private readonly PackedScene _mainScene = ResourceLoader.Load<PackedScene>("res://Main.tscn");

		public override void _Ready()
		{
			_startButton = GetNode<Button>("Base/StartButton");
			_startButton.Pressed += OnStartButtonPressed;

			_licensesButton = GetNode<Button>("Base/LicensesButton");
			_licensesButton.Pressed += OnLicensesButtonPressed;

			_licensesControl = GetNode<Control>("Licenses");

			_closeButton = GetNode<Button>("Licenses/MarginContainer/VBoxContainer/CloseButton");
			_closeButton.Pressed += OnCloseButtonPressed;

			_licenseText = GetNode<RichTextLabel>("Licenses/MarginContainer/VBoxContainer/Panel/MarginContainer/LicenseText");

			LoadLicenseText();
		}

		private void LoadLicenseText()
		{
			StringBuilder stringBuilder = new("[b]Godot Engine[/b]\n");
			stringBuilder.Append($"{Engine.GetLicenseText()}\n");

			foreach (var (key, value) in Engine.GetLicenseInfo())
			{
				stringBuilder.Append($"[b]{key}[/b]\n");
				stringBuilder.Append($"{value}\n");
			}

			// Get dictionaries from an Array of Dictionaries
			foreach (var copyrightInfo in Engine.GetCopyrightInfo())
			{
				// Dictionary has two keys "name" and "parts"
				// "name" is a string with the name of the component
				// "parts" is another array of dictionaries
				stringBuilder.Append($"[b]{copyrightInfo["name"]}[/b]\n");
				foreach (var part in (Godot.Collections.Array)copyrightInfo["parts"])
				{
					// Each part contains "files," "copyright," and "license"
					Godot.Collections.Dictionary partDictionary = (Godot.Collections.Dictionary)part;

					// "files" is an array
					stringBuilder.Append("Files: ");
					foreach (var file in (Godot.Collections.Array)partDictionary["files"])
					{
						stringBuilder.Append($"{file}\n");
					}

					// "copyright" is an array
					stringBuilder.Append("Copyright: ");
					foreach (var copyright in (Godot.Collections.Array)partDictionary["copyright"])
					{
						stringBuilder.Append($"{copyright}\n");
					}

					// "license" is a string
					stringBuilder.Append("License: ");
					stringBuilder.Append($"{partDictionary["license"]}\n");
				}
				stringBuilder.Append('\n');
			}

			stringBuilder.Append("[b]Press Start 2P Font[/b]\n");
			using (FileAccess licenseFile = FileAccess.Open(_fontLicensePath, FileAccess.ModeFlags.Read))
			{
				stringBuilder.Append($"{licenseFile.GetAsText()}\n");
			}

			_licenseText.Text = stringBuilder.ToString();
		}

		private void OnStartButtonPressed()
		{
			GetTree().ChangeSceneToPacked(_mainScene);
		}

		private void OnLicensesButtonPressed()
		{
			_licensesControl.Show();
		}

		private void OnCloseButtonPressed()
		{
			_licensesControl.Hide();
		}
	}
}
