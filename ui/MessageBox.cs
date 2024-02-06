using Godot;
using System.Threading.Tasks;

namespace Platypus.UserInterface;

public partial class MessageBox : Control
{
	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("MessageLabel");
	}

	public async Task DisplayMessage(string text, int millisecondsDelay = 3000)
	{
		_label.Text = text;
		Show();
		await Task.Delay(millisecondsDelay);
		Hide();
	}
}
