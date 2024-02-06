using Godot;
using System;

namespace Platypus.UserInterface
{
    public partial class MainMenu : Control
    {
        private Button _startButton;
        private Button _licensesButton;
        private Button _closeButton;
        private RichTextLabel _licenseText;

        public override void _Ready()
        {
            _startButton = GetNode<Button>("Base/StartButton");
            _startButton.Pressed += OnStartButtonPressed;
            _closeButton = GetNode<Button>("Licenses/MarginContainer/VBoxContainer/CloseButton");
            _closeButton.Pressed += OnCloseButtonPressed;
            _licenseText = GetNode<RichTextLabel>("Licenses/MarginContainer/VBoxContainer/Panel/LicenseText");

            _licenseText.Text = Engine.GetLicenseText();
        }

        private void OnStartButtonPressed()
        {
            GD.Print("Start button clicked!");
        }

        private void OnCloseButtonPressed()
        {
            GD.Print("Close pressed");
        }
    }
}
